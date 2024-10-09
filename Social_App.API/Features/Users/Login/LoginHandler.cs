using Social_App.API.CQRSConfigurations;
using Social_App.Services.Interfaces;

namespace Social_App.API.Features.Users.Login
{
    public class LoginHandler
        (IUserManagerWithMarten userManager)
        : ICommandHandler<LoginRequest, LoginResponse>
    {
        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var result = await userManager.LoginWithEmailOrUserName(request.Email, request.Password);
            return new LoginResponse
            {
                Success = result is not null,
                TokenModel = result
            };
        }
    }
}
