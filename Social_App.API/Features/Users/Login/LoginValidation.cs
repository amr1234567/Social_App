using FluentValidation;

namespace Social_App.API.Features.Users.Login
{
    public class LoginValidation : AbstractValidator<LoginRequest>
    {
        public LoginValidation()
        {
            RuleFor(c => c.Email)
                .NotEmpty()
                .WithMessage("user name or email is required field")
                .Matches("^([a-zA-Z0-9-_]+|[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,})$")
                .WithMessage("You must enter a valid user name or valid email");

            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .Matches("^[a-zA-Z0-9!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]*$")
                .WithMessage("Password must contain a letter, number and special char");
        }
    }
}
