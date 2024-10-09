using FluentValidation;
using Social_App.API.Interfaces;

namespace Social_App.API.Features.Users.ChangePassword
{
    public class ChangePasswordValidation : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidation(IAccountServices accountServices)
        {
            RuleFor(u => u.NewPassword)
                .NotEmpty()
                .WithMessage("Password is required")
                .Matches("^[a-zA-Z0-9!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]*$\r\n")
                .WithMessage("Password must contain a letter, number and special char");

            // Username validation (can be an email or a username)
            RuleFor(u => u.UserName)
                .NotEmpty()
                .WithMessage("Username is required")
                .Must(accountServices.BeAValidUserNameOrEmail)
                .WithMessage("Username must be a valid email or a username (letters, numbers, hyphens, underscores)");

            RuleFor(c => c.VerifecationCode)
                .NotEmpty()
                .WithMessage("Verifecation Code is required");
        }
    }
}
