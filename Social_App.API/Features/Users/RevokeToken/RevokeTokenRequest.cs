using Social_App.API.CQRSConfigurations;

namespace Social_App.API.Features.Users.RevokeToken
{
    public class RevokeTokenRequest: ICommand<RevokeTokenResponse>
    {
        public string RefreshToken { get; set; }
    }
}
