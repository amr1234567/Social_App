using Social_App.API.CQRSConfigurations;

namespace Social_App.API.Features.Users.ChangePassword
{
    public class ChangePasswordRequest : ICommand<ChangePasswordResponse>
    {
        public string UserName { get; set; }
        public string NewPassword { get; set; }
        public string VerifecationCode { get; set; }
    }
}
