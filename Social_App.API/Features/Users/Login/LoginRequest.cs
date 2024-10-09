using Social_App.API.CQRSConfigurations;

namespace Social_App.API.Features.Users.Login
{
    public class LoginRequest : ICommand<LoginResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
