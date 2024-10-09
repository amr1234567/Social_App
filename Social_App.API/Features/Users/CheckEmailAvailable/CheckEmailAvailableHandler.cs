using Social_App.API.CQRSConfigurations;
using Social_App.Services.Interfaces;

namespace Social_App.API.Features.Users.CheckEmailAvaliable
{
    public class CheckEmailAvailableHandler
        (IUserManagerWithMarten userManager)
        : IQueryHandler<CheckEmailAvailableRequest, CheckEmailAvailableResponse>
    {
        public async Task<CheckEmailAvailableResponse> Handle(CheckEmailAvailableRequest request, CancellationToken cancellationToken)
        {
            var result = await userManager.DoesEmailExists(request.Email);
            return new CheckEmailAvailableResponse { Available = !result };
        }
    }
}
