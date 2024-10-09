using Social_App.API.CQRSConfigurations;
using Social_App.Services.Interfaces;

namespace Social_App.API.Features.Users.CheckUserNameAvaliable
{
    public class CheckUserNameAvailableHandler
        (IUserManagerWithMarten userManager)
        : IQueryHandler<CheckUserNameAvailableRequest, CheckUserNameAvailableResponse>
    {
        public async Task<CheckUserNameAvailableResponse> Handle(CheckUserNameAvailableRequest request, CancellationToken cancellationToken)
        {
            var result = await userManager.DoesUserNameExists(request.UserName);
            return new CheckUserNameAvailableResponse { Available = !result };
        }
    }
}
