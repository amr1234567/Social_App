


namespace Social_App.API.Features.Users.VerifyAccount
{
    public class VerifyAccountHandler
        (ApplicationContext context,IAccountServices accountServices,ILogger<VerifyAccountHandler> logger)
        : ICommandHandler<VerifyAccountRequest, VerifyAccountResponse>
    {
        public async Task<VerifyAccountResponse> Handle(VerifyAccountRequest request, CancellationToken cancellationToken)
        {
            var result = await VerifyAccount(request.UserName, request.VerifecationCode);
            return new VerifyAccountResponse { Success = result };
        }

        private async Task<bool> VerifyAccount(string userName, string verifecationCode)
        {
            var user = await context.Users
                    .FirstOrDefaultAsync(u => u.UserName.Equals(userName) || u.Email.Equals(userName))
                    ?? throw new NotFoundException(typeof(User).Name, userName);
            if (accountServices.HashString(verifecationCode) == user.VerifecationCode)
            {
                user.VerifecationCode = "";
                user.IsEmailConfirmed = true;
                context.Users.Update(user);
                await context.SaveChangesAsync();
                logger.LogInformation("(User) Account Verified");
                return true;
            }
            logger.LogError("(User) Codes Doesn't match");
            return false;
        }

        #region Change data with marten
        //private async Task<bool> VerifyAccount(string userName, string verifecationCode)
        //{
        //    var user = await session.Query<User>()
        //            .FirstOrDefaultAsync(u => u.UserName.Equals(userName) || u.Email.Equals(userName))
        //            ?? throw new NotFoundException(typeof(User).Name, userName);
        //    if (accountServices.HashString(verifecationCode) == user.VerifecationCode)
        //    {
        //        user.VerifecationCode = "";
        //        user.IsEmailConfirmed = true;
        //        session.Update(user);
        //        await session.SaveChangesAsync();
        //        logger.LogInformation("(User) Account Verified");
        //        return true;
        //    }
        //    logger.LogError("(User) Codes Doesn't match");
        //    return false;
        //} 
        #endregion

    }
}
