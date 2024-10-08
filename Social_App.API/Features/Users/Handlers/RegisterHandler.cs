using FluentValidation;
using Mapster;
using MediatR;
using Social_App.API.CQRSConfigurations;
using Social_App.API.Features.Users.DtoModels;
using Social_App.Core.Identity;
using Social_App.Services.Interfaces;

namespace Social_App.API.Features.Users.Handlers
{

    public class RegisterValidation : AbstractValidator<CreateUserCommand>
    {
        public RegisterValidation()
        {
            RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email must be valid");

            RuleFor(u => u.UserName)
                .NotEmpty()
                .WithMessage("UserName is required")
                .Matches("^[a-zA-Z0-9!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]*$\r\n")
                .WithMessage("User name must contain a letter, number and special char");

            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .Matches("^[a-zA-Z0-9!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]*$\r\n")
                .WithMessage("Password must contain a letter, number and special char");

            RuleFor(u => u.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Confirm Password is required")
                .Equal(u => u.Password)
                .WithMessage("Confirm Passowrd Field must be the same as password field");

            RuleFor(u => u.FirstName)
                .NotEmpty()
                .WithMessage("First Name is required")
                .Matches("^[a-zA-Z]+$\r\n")
                .WithMessage("First Name must contain only letters");

            RuleFor(u => u.LastName)
                .NotEmpty()
                .WithMessage("Last Name is required")
                .Matches("^[a-zA-Z]+$\r\n")
                .WithMessage("Last Name must contain only letters");


            RuleFor(u => u.Gender)
                .NotEmpty()
                .WithMessage("Gender is required")
                .Must(g => g == 'm' || g == 'M' || g == 'f' || g == 'F')
                .WithMessage("You must enter the gender as one of these values: (M,m for male - F,f for female)");

        }
    }

    public class RegisterHandler(IUserManagerWithMarten userManager)
        : ICommandHandler<CreateUserCommand,CreateUserResponse>
    {
        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = request.Adapt<User>();

            var result = await userManager.RegisterAccount(user);
            return new CreateUserResponse
            {
                VerifecationCode = result
            };
        }
    }
}
