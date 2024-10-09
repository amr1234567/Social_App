using FluentValidation;

namespace Social_App.API.Features.Users.RefreshToken
{
    public class RefreshTokenValidation : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenValidation()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .WithMessage("Refresh token is required");

            RuleFor(x => x.AccessToken)
                .NotEmpty()
                .WithMessage("Access token is required");
        }
    }
}
