using Social_App.API.CQRSConfigurations;

namespace Social_App.API.Features.Users.CheckEmailAvaliable
{
    public class CheckEmailAvailableRequest : IQuery<CheckEmailAvailableResponse>
    {
        public string Email { get; set; }
    }
}
