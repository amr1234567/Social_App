using FluentValidation;
using Social_App.API.Features.Users.CheckUserNameAvaliable;

namespace Social_App.API.Features.Users.CheckUserNameAvailable
{
    public class CheckUserNameAvailableValidation : AbstractValidator<CheckUserNameAvailableRequest>
    {
        public CheckUserNameAvailableValidation()
        {
            RuleFor(u => u.UserName)
               .NotEmpty()
               .WithMessage("UserName is required")
               .Matches("^[a-zA-Z0-9-_]+$\r\n")
               .WithMessage("User name not valid");
        }
    }
}
