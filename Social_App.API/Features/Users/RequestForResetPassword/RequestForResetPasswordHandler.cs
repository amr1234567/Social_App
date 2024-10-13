


namespace Social_App.API.Features.Users.RequestForResetPassword
{
    public class RequestForResetPasswordHandler 
        (ApplicationContext context,IAccountServices accountServices, ILogger<RequestForResetPasswordHandler> logger)
        : ICommandHandler<RequestForResetPasswordRequest, RequestForResetPasswordResponse>
    {
        private int maxNumOfTries = 4;
        public async Task<RequestForResetPasswordResponse> Handle(RequestForResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await RequestForResetPassword(request.UserName);
            return new RequestForResetPasswordResponse { Success = result };
        }

        private async Task<bool> RequestForResetPassword(string userName)
        {
            var user = await context.Users
                   .FirstOrDefaultAsync(u => u.UserName.Equals(userName) || u.Email.Equals(userName))
                   ?? throw new NotFoundException(typeof(User).Name, userName);
            user.NumberOfTries += 1;
            if (user.NumberOfTries > maxNumOfTries)
            {
                user.AccountLocked = true;
                context.Users.Update(user);
                await context.SaveChangesAsync();
                throw new Exception("User Rech The max number of tries");
            }

            var code = accountServices.CreateVerifecationCode(6);
            logger.LogInformation($"[Verifecation Code For Password] ==> '{code}'");
            user.VerifecationCode = accountServices.HashString(code);
            user.FinalTimeForVerify = DateTime.Now.AddMinutes(60);

            //await emailSender.SendEmailAsync(user.Email, "Verify your account", $"<h2>Your verifecation code is <strong>{code}</strong></h2>");

            context.Users.Update(user);
            await context.SaveChangesAsync();
            return true;
        }

        #region Saving data with marten
        //private async Task<bool> RequestForResetPassword(string userName)
        //{
        //    var user = await session.Query<User>()
        //            .FirstOrDefaultAsync(u => u.UserName.Equals(userName) || u.Email.Equals(userName))
        //            ?? throw new NotFoundException(typeof(User).Name, userName);
        //    user.NumberOfTries += 1;
        //    if (user.NumberOfTries > maxNumOfTries)
        //    {
        //        user.AccountLocked = true;
        //        session.Update(user);
        //        await session.SaveChangesAsync();
        //        throw new Exception("User Rech The max number of tries");
        //    }

        //    var code = accountServices.CreateVerifecationCode(6);
        //    logger.LogInformation($"[Verifecation Code For Password] ==> '{code}'");
        //    user.VerifecationCode = accountServices.HashString(code);
        //    user.FinalTimeForVerify = DateTime.Now.AddMinutes(60);

        //    //await emailSender.SendEmailAsync(user.Email, "Verify your account", $"<h2>Your verifecation code is <strong>{code}</strong></h2>");

        //    session.Update(user);
        //    await session.SaveChangesAsync();
        //    return true;
        //} 
        #endregion

    }
}
