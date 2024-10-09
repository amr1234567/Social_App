using Social_App.API.CQRSConfigurations;

namespace Social_App.API.Features.Users.VerifyAccount
{
    public class VerifyAccountRequest : ICommand<VerifyAccountResponse>
    {
        public string UserName { get; set; }
        public string VerifecationCode { get; set; }
    }
}
