using Marten;
using Social_App.API.CQRSConfigurations;
using Social_App.API.Features.Users.Login;
using Social_App.API.Interfaces;
using Social_App.API.Models.Exceptions;
using Social_App.API.Models.Helpers;
using Social_App.API.Models.Identity;

namespace Social_App.API.Features.Users.RefreshToken
{
    public class RefreshTokenHandler
        (IDocumentSession session, IAccountServices accountServices)
        : ICommandHandler<RefreshTokenRequest, RefreshTokenResponse>
    {
        public async Task<RefreshTokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await RefreshTheToken(request.AccessToken, request.RefreshToken);
            return new RefreshTokenResponse
            {
                Success = result is not null,
                TokenModel = result
            };
        }

        private async Task<TokenModel> RefreshTheToken(string accessToken, string refreshToken)
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

    }
}
