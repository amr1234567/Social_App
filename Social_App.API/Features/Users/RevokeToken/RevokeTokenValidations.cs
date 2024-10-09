using FluentValidation;

namespace Social_App.API.Features.Users.RevokeToken
{
    public class RevokeTokenValidations : AbstractValidator<RevokeTokenRequest>
    {
        public RevokeTokenValidations()
        {
            RuleFor(c => c.RefreshToken)
                .NotEmpty()
                .WithMessage("Refresh token is required");
        }
    }
}
