using Marten;
using Social_App.API.CQRSConfigurations;
using Social_App.API.Interfaces;
using Social_App.API.Models.Exceptions;
using Social_App.API.Models.Helpers;
using Social_App.API.Models.Identity;
using System.Security.Claims;

namespace Social_App.API.Features.Users.Login
{
    public class LoginHandler
        (IDocumentSession session,IAccountServices accountServices)
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

        private async Task<TokenModel> LoginWithEmailOrUserName(string userName, string password)
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

    }
}
