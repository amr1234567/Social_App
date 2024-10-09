using Social_App.API.CQRSConfigurations;
using Social_App.Services.Interfaces;

namespace Social_App.API.Features.Users.RequestForResetPassword
{
    public class RequestForResetPasswordHandler 
        (IUserManagerWithMarten userManager)
        : ICommandHandler<RequestForResetPasswordRequest, RequestForResetPasswordResponse>
    {
        public async Task<RequestForResetPasswordResponse> Handle(RequestForResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await userManager.RequestForResetPassword(request.UserName);
            return new RequestForResetPasswordResponse { Success = result };
        }
    }
}
