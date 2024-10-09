using Social_App.API.CQRSConfigurations;
using Social_App.Services.Interfaces;

namespace Social_App.API.Features.Users.ChangePassword
{
    public class ChangePasswordHandler
        (IUserManagerWithMarten userManager)
        : ICommandHandler<ChangePasswordRequest, ChangePasswordResponse>
    {
        public async Task<ChangePasswordResponse> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await userManager.ChangePassword(request.UserName, request.NewPassword, request.VerifecationCode);
            return new ChangePasswordResponse { Success = result };
        }
    }
}
