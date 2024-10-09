using Social_App.API.CQRSConfigurations;
using Social_App.API.Features.Users.Login;
using Social_App.Services.Interfaces;

namespace Social_App.API.Features.Users.RefreshToken
{
    public class RefreshTokenHandler
        (IUserManagerWithMarten userManager)
        : ICommandHandler<RefreshTokenRequest, RefreshTokenResponse>
    {
        public async Task<RefreshTokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await userManager.RefreshTheToken(request.AccessToken, request.RefreshToken);
            return new RefreshTokenResponse
            {
                Success = result is not null,
                TokenModel = result
            };
        }
    }
}
