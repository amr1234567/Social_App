using FluentValidation;
using Mapster;
using MediatR;
using Social_App.API.CQRSConfigurations;
using Social_App.Core.Identity;
using Social_App.Services.Interfaces;

namespace Social_App.API.Features.Users.RegisterUser
{
    public class RegisterHandler(IUserManagerWithMarten userManager)
        : ICommandHandler<CreateUserCommand, CreateUserResponse>
    {
        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = request.Adapt<User>();

            var result = await userManager.RegisterAccount(user);
            return new CreateUserResponse
            {
                Message = result ? "Created Successfully" : "Failed",
                Success = result,
            };
        }
    }
}
