using Social_App.Core.Helpers;

namespace Social_App.API.Features.Users.Login
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public TokenModel? TokenModel { get; set; }
    }
}
