﻿using FluentValidation;
using Social_App.API.Interfaces;

namespace Social_App.API.Features.Users.VerifyChangingPasswordCodeToken
{
    public class VerifyChangingPasswordCodeTokenValidation : AbstractValidator<VerifyChangingPasswordCodeTokenRequest>
    {
        public VerifyChangingPasswordCodeTokenValidation(IAccountServices accountServices)
        {
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
