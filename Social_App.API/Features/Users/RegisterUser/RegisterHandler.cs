﻿using FluentValidation;
using Mapster;

namespace Social_App.API.Features.Users.RegisterUser
{
    public class RegisterHandler
        (ApplicationContext context, IAccountServices accountServices, ILogger<RegisterHandler> logger)
        : ICommandHandler<RegisterRequest, RegisterResponse>
    {
        public async Task<RegisterResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            var user = request.Adapt<User>();

            var result = await RegisterAccount(user);
            return new RegisterResponse
            {
                Message = result ? "Created Successfully" : "Failed",
                Success = result,
            };
        }

        private async Task<bool> RegisterAccount(User user)
        {
            var userExistCheck = await context.Users
                .Where(u => u.Email.Equals(user.Email) || u.UserName.Equals(user.UserName))
                .AnyAsync();
            if (userExistCheck)
                throw new BadRequestException($"User with this email or user name is already exist.");

            var salt = accountServices.CreateSalt();
            var hashedPassword = accountServices.HashPasswordWithSalt(salt, user.Password);

            user.Password = hashedPassword;
            user.Salt = salt;

            var code = accountServices.CreateVerifecationCode(6);

            user.VerifecationCode = accountServices.HashString(code);

            //delete this line after debug
            logger.LogInformation($"[Verifecation Code] ==> '{code}'");
            //await emailSender.SendEmailAsync(user.Email, "Verify your account", $"<h2>Your verifecation code is <strong>{code}</strong></h2>");

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            logger.LogInformation("(User) Account Created");
            return true;
        }

        #region Saving accounts with marten
        //private async Task<bool> RegisterAccount(User user)
        //{
        //    var userExistCheck = await session.Query<User>()
        //        .Where(u => u.Email.Equals(user.Email) || u.UserName.Equals(user.UserName))
        //        .AnyAsync();
        //    if (userExistCheck)
        //        throw new BadRequestException($"User with this email or user name is already exist.");

        //    var salt = accountServices.CreateSalt();
        //    var hashedPassword = accountServices.HashPasswordWithSalt(salt, user.Password);

        //    user.Password = hashedPassword;
        //    user.Salt = salt;

        //    var code = accountServices.CreateVerifecationCode(6);

        //    user.VerifecationCode = accountServices.HashString(code);

        //    //delete this line after debug
        //    logger.LogInformation($"[Verifecation Code] ==> '{code}'");
        //    //await emailSender.SendEmailAsync(user.Email, "Verify your account", $"<h2>Your verifecation code is <strong>{code}</strong></h2>");

        //    session.Store(user);
        //    await session.SaveChangesAsync();
        //    logger.LogInformation("(User) Account Created");
        //    return true;
        //} 
        #endregion

    }
}
