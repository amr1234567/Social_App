using Social_App.Core.Helpers;

namespace Social_App.API.Features.Users.RefreshToken
{
    public class RefreshTokenResponse
    {
        public bool Success { get; set; }
        public TokenModel? TokenModel { get; set; }
    }
}
