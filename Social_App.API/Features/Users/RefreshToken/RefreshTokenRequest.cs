using Social_App.API.CQRSConfigurations;

namespace Social_App.API.Features.Users.RefreshToken
{
    public class RefreshTokenRequest : ICommand<RefreshTokenResponse>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
