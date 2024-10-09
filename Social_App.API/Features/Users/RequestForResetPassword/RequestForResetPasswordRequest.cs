using Social_App.API.CQRSConfigurations;

namespace Social_App.API.Features.Users.RequestForResetPassword
{
    public class RequestForResetPasswordRequest : ICommand<RequestForResetPasswordResponse>
    {
        public string UserName { get; set; }
    }
}
