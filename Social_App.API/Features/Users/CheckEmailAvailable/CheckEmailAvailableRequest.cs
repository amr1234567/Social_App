using Social_App.API.CQRSConfigurations;

namespace Social_App.API.Features.Users.CheckEmailAvailable
{
    public class CheckEmailAvailableRequest : IQuery<CheckEmailAvailableResponse>
    {
        public string Email { get; set; }
    }
}
