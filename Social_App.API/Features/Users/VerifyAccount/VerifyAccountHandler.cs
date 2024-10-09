using Social_App.API.CQRSConfigurations;
using Social_App.Services.Interfaces;

namespace Social_App.API.Features.Users.VerifyAccount
{
    public class VerifyAccountHandler
        (IUserManagerWithMarten userManager)
        : ICommandHandler<VerifyAccountRequest, VerifyAccountResponse>
    {
        public async Task<VerifyAccountResponse> Handle(VerifyAccountRequest request, CancellationToken cancellationToken)
        {
            var result = await userManager.VerifyAccount(request.UserName, request.VerifecationCode);
            return new VerifyAccountResponse { Success = result };
        }
    }
}
