using Social_App.API.CQRSConfigurations;
using Social_App.Services.Interfaces;

namespace Social_App.API.Features.Users.RevokeToken
{
    public class RevokeTokenHandler
        (IUserManagerWithMarten userManager)
        : ICommandHandler<RevokeTokenRequest, RevokeTokenResponse>
    {
        public async Task<RevokeTokenResponse> Handle(RevokeTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await userManager.RevokeToken(request.RefreshToken);
            return new RevokeTokenResponse(result);
        }
    }
}
