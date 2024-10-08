using JasperFx.CodeGeneration.Frames;
using Marten;
using Microsoft.IdentityModel.Tokens;
using Social_App.Core.Exceptions;
using Social_App.Core.Helpers;
using Social_App.Core.Identity;
using Social_App.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Social_App.Services.IdentityServices
{
    public class UserManagerWithMarten
        (IDocumentSession session, IAccountServices accountServices, IEmailSender emailSender) 
        : IUserManagerWithMarten
    {
        public async Task<bool> EmailExists(string email)
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

        public Task<TokenModel> LoginWithEmailOrUserName(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<TokenModel> RefreshTheToken(string accessToken, string refreshToken)
        {
            throw new NotImplementedException();
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

            await emailSender.SendEmailAsync(user.Email, "Verify your account", $"<h2>Your verifecation code is <strong>{code}</strong></h2>");
            
            session.Store(user);
            await session.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RevokeToken(string refreshToken)
        {
            var user = await session.Query<User>()
                .FirstOrDefaultAsync(u => !string.IsNullOrEmpty(u.RefreshToken) && u.RefreshToken.Equals(refreshToken))
                ?? throw new NotFoundException($"User with refresh Token '{refreshToken}' not found");
            user.RefreshToken = null;
            return true;
        }

        public async Task<bool> UserNameExists(string userName)
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
                return true;
            }
            return false;
        }

        
    }
}
