
using System.Security.Claims;

namespace Social_App.API.Features.Users.Login
{
    public class LoginHandler
        (ApplicationContext context,IAccountServices accountServices)
        : ICommandHandler<LoginRequest, LoginResponse>
    {
        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var result = await LoginWithEmailOrUserName(request.Email, request.Password);
            return new LoginResponse
            {
                Success = result is not null,
                TokenModel = result
            };
        }

        private async Task<TokenModel> LoginWithEmailOrUserName(string email, string password)
        {
            var user = await context.Users
                    .FirstOrDefaultAsync(u => u.UserName.Equals(email) || u.Email.Equals(email))
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

            context.Users.Update(user);
            await context.SaveChangesAsync();

            return new TokenModel
            {
                RefreshToken = refreshToken,
                AccessToken = accessToken
            };
        }

        #region Saving data with marten
        //private async Task<TokenModel> LoginWithEmailOrUserName(string userName, string password)
        //{
        //    var user = await session.Query<User>()
        //            .FirstOrDefaultAsync(u => u.UserName.Equals(userName) || u.Email.Equals(userName))
        //            ?? throw new NotFoundException("Email or password wrong");
        //    if (accountServices.HashPasswordWithSalt(user.Salt, password) != user.Password)
        //        throw new NotFoundException("Email or password wrong");

        //    if (!user.IsEmailConfirmed)
        //    {
        //        throw new Exception("Forbbiden Account");
        //    }
        //    var claims = new List<Claim>
        //    {
        //        new(ClaimTypes.NameIdentifier,user.Id.ToString()),
        //        new(ClaimTypes.Name,user.UserName),
        //        new(CustomClaimTypes.FullUserName,user.FirstName+user.LastName),
        //        new(ClaimTypes.Email,user.Email),
        //        new(CustomClaimTypes.Gender, user.Gender.ToString())
        //    };

        //    var accessToken = accountServices.CreateJwtToken(claims);
        //    var refreshToken = accountServices.CreateRefreshToken();
        //    user.RefreshToken = refreshToken;

        //    session.Update(user);
        //    await session.SaveChangesAsync();

        //    return new TokenModel
        //    {
        //        RefreshToken = refreshToken,
        //        AccessToken = accessToken
        //    };
        //}

        #endregion
    }
}
