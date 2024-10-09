using Social_App.API.CQRSConfigurations;

namespace Social_App.API.Features.Users.CheckUserNameAvailable
{
    public class CheckUserNameAvailableRequest : IQuery<CheckUserNameAvailableResponse>
    {
        public string UserName { get; set; }
    }
}
