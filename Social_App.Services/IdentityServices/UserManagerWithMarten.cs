using Microsoft.Extensions.Logging;
using Social_App.Services.Interfaces;
using System.Security.Claims;

namespace Social_App.Services.IdentityServices
{
    public class UserManagerWithMarten
        (IDocumentSession session, IAccountServices accountServices, IEmailSender emailSender, ILogger<UserManagerWithMarten> logger) 
        : IUserManagerWithMarten
    {
        private const int maxNumOfTries = 5;

        public async Task<bool> DoesEmailExists(string email)
        {
            return await session.Query<User>()
                   .Where(u => u.Email.Equals(email))
                   .AnyAsync();
        }

        public async Task<User> FindUserByEmailOrUserName(string userName)
        {
            return await session.Query<User>()
                    .FirstOrDefaultAsync(u => u.UserName.Equals(userName) || u.Email.Equals(userName))
                    ?? throw new NotFoundException(typeof(User).Name, userName);

        }

        public async Task<bool> IsUserActive(string userName)
        {
            var user = await session.Query<User>()
                    .FirstOrDefaultAsync(u => u.UserName.Equals(userName) || u.Email.Equals(userName));
            return user is null ? throw new NotFoundException(typeof(User).Name, userName) : user.IsEmailConfirmed;
        }

        public async Task<TokenModel> LoginWithEmailOrUserName(string userName, string password)
        {
            var user = await session.Query<User>()
                    .FirstOrDefaultAsync(u => u.UserName.Equals(userName) || u.Email.Equals(userName))
                    ?? throw new NotFoundException("Email or password wrong");
            if (accountServices.HashPasswordWithSalt(user.Salt, password) != user.Password)
                throw new NotFoundException("Email or password wrong");

            if (!user.IsEmailConfirmed)
            {
                throw new Exception("Forbbiden Account");
            }
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new(ClaimTypes.Name,user.UserName),
                new(CustomClaimTypes.FullUserName,user.FirstName+user.LastName),
                new(ClaimTypes.Email,user.Email),
                new(CustomClaimTypes.Gender, user.Gender.ToString())
            };

            var accessToken = accountServices.CreateJwtToken(claims);
            var refreshToken = accountServices.CreateRefreshToken();
            user.RefreshToken = refreshToken;
            session.Update(user);
            await session.SaveChangesAsync();

            return new TokenModel
            {
                RefreshToken = refreshToken,
                AccessToken = accessToken
            };
        }

        public async Task<TokenModel> RefreshTheToken(string accessToken, string refreshToken)
        {
            var user = await session.Query<User>()
               .FirstOrDefaultAsync(u => !string.IsNullOrEmpty(u.RefreshToken) && u.RefreshToken.Equals(refreshToken))
               ?? throw new NotFoundException($"User with refresh Token '{refreshToken}' not found");

            var newRefreshToken = accountServices.CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            var pricples = accountServices.GetPrincipalFromToken(accessToken);
            var newToken = accountServices.GenerateTokenFromPrincipal(pricples);

            session.Update(user);
            await session.SaveChangesAsync();

            return new TokenModel
            {
                AccessToken = newToken,
                RefreshToken = newRefreshToken,
            };
        }

        public async Task<bool> RegisterAccount(User user)
        {
            var userExistCheck = await session.Query<User>()
                .Where(u => u.Email.Equals(user.Email) || u.UserName.Equals(user.UserName))
                .AnyAsync();
            if (userExistCheck)
                throw new BadRequestException($"User with this email or user name is already exist.");

            var salt = accountServices.CreateSalt();
            var hashedPassword = accountServices.HashPasswordWithSalt(salt, user.Password);

            user.Password = hashedPassword;
            user.Salt = salt;

           
            var code = accountServices.CreateVerifecationCode(6);

            user.VerifecationCode = accountServices.HashString(code);

            //delete this line after debug
            logger.LogInformation($"[Verifecation Code] ==> '{code}'");
            //await emailSender.SendEmailAsync(user.Email, "Verify your account", $"<h2>Your verifecation code is <strong>{code}</strong></h2>");
            
            session.Store(user);
            await session.SaveChangesAsync();
            logger.LogInformation("(User) Account Created");
            return true;
        }

        public async Task<bool> RevokeToken(string refreshToken)
        {
            var user = await session.Query<User>()
                .FirstOrDefaultAsync(u => !string.IsNullOrEmpty(u.RefreshToken) && u.RefreshToken.Equals(refreshToken))
                ?? throw new NotFoundException($"User with refresh Token '{refreshToken}' not found");
            user.RefreshToken = null;
            session.Update(user);
            await session.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DoesUserNameExists(string userName)
        {
            return await session.Query<User>()
                    .Where(u => u.UserName.Equals(userName))
                    .AnyAsync();
        }

        public async Task<bool> VerifyAccount(string username, string verifecationCode)
        {
            var user = await FindUserByEmailOrUserName(username);
            if (accountServices.HashString(verifecationCode) == user.VerifecationCode)
            {
                user.VerifecationCode = "";
                user.IsEmailConfirmed = true;
                session.Update(user);
                await session.SaveChangesAsync();
                logger.LogInformation("(User) Account Verified");
                return true;
            }
            logger.LogError("(User) Codes Doesn't match");
            return false;
        }

        //for the first screen that contain a field for email
        public async Task<bool> RequestForResetPassword(string email)
        {
            var user = await FindUserByEmailOrUserName(email);
            user.NumberOfTries += 1;
            if (user.NumberOfTries > maxNumOfTries)
            {
                user.AccountLocked = true;
                session.Update(user);
                await session.SaveChangesAsync();
                throw new Exception("User Rech The max number of tries");
            }

            var code = accountServices.CreateVerifecationCode(6);
            logger.LogInformation($"[Verifecation Code For Password] ==> '{code}'");
            user.VerifecationCode = accountServices.HashString(code);
            user.FinalTimeForVerify = DateTime.Now.AddMinutes(60);

            //await emailSender.SendEmailAsync(user.Email, "Verify your account", $"<h2>Your verifecation code is <strong>{code}</strong></h2>");

            session.Update(user);
            await session.SaveChangesAsync();
            return true;
        }

        //for second screen that contain a field for verifecation code
        public async Task<bool> VerifyChangingPasswordCodeToken(string email, string code)
        {
            var user = await FindUserByEmailOrUserName(email);
            if (user.VerifecationCode != accountServices.HashString(code))
                return false;
            return true;
        }
        
        //for third screen that contain 2 fields for password and confirm password
        public async Task<bool> ChangePassword(string email, string newPassword, string code)
        {
            var user = await FindUserByEmailOrUserName(email);
            if (user.VerifecationCode != accountServices.HashString(code))
                return false;
            var salt = accountServices.CreateSalt();
            user.Password = accountServices.HashPasswordWithSalt(salt, newPassword);
            user.Salt = salt;
            session.Update(user);
            await session.SaveChangesAsync();
            return true;
        }
    }
}
