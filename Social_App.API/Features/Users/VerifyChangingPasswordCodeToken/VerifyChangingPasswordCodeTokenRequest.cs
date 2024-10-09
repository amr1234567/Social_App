using Social_App.API.CQRSConfigurations;

namespace Social_App.API.Features.Users.VerifyChangingPasswordCodeToken
{
    public class VerifyChangingPasswordCodeTokenRequest : IQuery<VerifyChangingPasswordCodeTokenResponse>
    {
        public string UserName { get; set; }
        public string VerifecationCode { get; set; }
    }
}
