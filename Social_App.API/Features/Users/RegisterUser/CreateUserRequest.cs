using Social_App.API.CQRSConfigurations;

namespace Social_App.API.Features.Users.RegisterUser
{
    public class CreateUserRequest : ICommand<CreateUserResponse>
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }
        public string ConfirmPassword { get; set; }


        public char Gender { get; set; }

    }
}
