using Social_App.API.CQRSConfigurations;
using Social_App.Services.Interfaces;

namespace Social_App.API.Features.Users.VerifyChangingPasswordCodeToken
{
    public class VerifyChangingPasswordCodeTokenHandler
        (IUserManagerWithMarten userManager)
        : IQueryHandler<VerifyChangingPasswordCodeTokenRequest, VerifyChangingPasswordCodeTokenResponse>
    {
        public async Task<VerifyChangingPasswordCodeTokenResponse> Handle(VerifyChangingPasswordCodeTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await userManager.VerifyChangingPasswordCodeToken(request.UserName, request.VerifecationCode);
            return new VerifyChangingPasswordCodeTokenResponse { Authorized = result };
        }
    }
}
