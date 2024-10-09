using FluentValidation;
using Social_App.API.Features.Users.CheckEmailAvaliable;

namespace Social_App.API.Features.Users.CheckEmailAvailable
{
    public class CheckEmailIsAvailableValidation : AbstractValidator<CheckEmailAvailableRequest>
    {
        public CheckEmailIsAvailableValidation()
        {
            RuleFor(u => u.Email)
               .NotEmpty()
               .WithMessage("Email is required")
               .EmailAddress()
               .WithMessage("Email must be valid");
        }
    }
}
