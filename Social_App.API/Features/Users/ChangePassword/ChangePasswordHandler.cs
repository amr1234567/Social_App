
namespace Social_App.API.Features.Users.ChangePassword
{
    public class ChangePasswordHandler
        (ApplicationContext context, IAccountServices accountServices)
        : ICommandHandler<ChangePasswordRequest, ChangePasswordResponse>
    {
        public async Task<ChangePasswordResponse> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await ChangePassword(request.UserName, request.NewPassword, request.VerifecationCode);
            return new ChangePasswordResponse { Success = result };
        }

        private async Task<bool> ChangePassword(string userName, string newPassword, string verifecationCode)
        {
            var user = await context.Users
                    .FirstOrDefaultAsync(u => u.UserName.Equals(userName) || u.Email.Equals(userName))
                    ?? throw new NotFoundException(typeof(User).Name, userName);

            if (user.VerifecationCode != accountServices.HashString(verifecationCode))
                return false;
            var salt = accountServices.CreateSalt();
            user.Password = accountServices.HashPasswordWithSalt(salt, newPassword);
            user.Salt = salt;
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return true;
        }

        #region Save data with Marten
        //private async Task<bool> ChangePassword(string email, string newPassword, string code)
        //{
        //    var user = await session.Query<User>()
        //            .FirstOrDefaultAsync(u => u.UserName.Equals(email) || u.Email.Equals(email))
        //            ?? throw new NotFoundException(typeof(User).Name, email);
        //    if (user.VerifecationCode != accountServices.HashString(code))
        //        return false;
        //    var salt = accountServices.CreateSalt();
        //    user.Password = accountServices.HashPasswordWithSalt(salt, newPassword);
        //    user.Salt = salt;
        //    session.Update(user);
        //    await session.SaveChangesAsync();
        //    return true;
        //} 
        #endregion

    }
}
