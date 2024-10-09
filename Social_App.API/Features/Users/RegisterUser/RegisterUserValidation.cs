using FluentValidation;
using Social_App.Services.Interfaces;
using System.Text.RegularExpressions;
namespace Social_App.API.Features.Users.RegisterUser
{
    public class RegisterUserValidation : AbstractValidator<CreateUserCommand>
    {
        public RegisterUserValidation()
        {
            // Email validation
            RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email must be valid");

            // Username validation (can be an email or a username)
            RuleFor(u => u.UserName)
                .NotEmpty()
                .WithMessage("Username is required")
                .Matches("^[a-zA-Z0-9-_]+$\r\n")
                .WithMessage("Username must be a valid email or a username (letters, numbers, hyphens, underscores)");

            // Password validation (at least one letter, one number, one special character)
            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).+$")
                .WithMessage("Password must contain at least one letter, one number, and one special character");

            // Confirm Password validation
            RuleFor(u => u.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Confirm Password is required")
                .Equal(u => u.Password)
                .WithMessage("Confirm Password must match the Password");

            // First Name validation
            RuleFor(u => u.FirstName)
                .NotEmpty()
                .WithMessage("First Name is required")
                .Matches("^[a-zA-Z]+$")
                .WithMessage("First Name must contain only letters");

            // Last Name validation
            RuleFor(u => u.LastName)
                .NotEmpty()
                .WithMessage("Last Name is required")
                .Matches("^[a-zA-Z]+$")
                .WithMessage("Last Name must contain only letters");

            // Gender validation (char type)
            RuleFor(u => u.Gender)
                .NotEmpty()
                .WithMessage("Gender is required")
                .Must(g => g == 'M' || g == 'm' || g == 'F' || g == 'f')
                .WithMessage("Gender must be 'M/m' for male or 'F/f' for female");
        }

        
    }
}
