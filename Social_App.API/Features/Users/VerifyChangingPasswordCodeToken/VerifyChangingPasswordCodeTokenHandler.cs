

using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Social_App.API.Features.Users.VerifyChangingPasswordCodeToken
{
    public class VerifyChangingPasswordCodeTokenHandler
        (ApplicationContext context, IAccountServices accountServices)
        : IQueryHandler<VerifyChangingPasswordCodeTokenRequest, VerifyChangingPasswordCodeTokenResponse>
    {
        public async Task<VerifyChangingPasswordCodeTokenResponse> Handle(VerifyChangingPasswordCodeTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await VerifyChangingPasswordCodeToken(request.UserName, request.VerifecationCode);
            return new VerifyChangingPasswordCodeTokenResponse { Authorized = result };
        }

        private async Task<bool> VerifyChangingPasswordCodeToken(string userName, string verifecationCode)
        {
            var user = await context.Users
                    .FirstOrDefaultAsync(u => u.UserName.Equals(userName) || u.Email.Equals(userName))
                    ?? throw new NotFoundException(typeof(User).Name, userName);
            if (user.VerifecationCode != accountServices.HashString(verifecationCode))
                return false;
            return true;
        }

        #region Verify with marten
        //public async Task<bool> VerifyChangingPasswordCodeToken(string userName, string code)
        //{
        //    var user = await session.Query<User>()
        //            .FirstOrDefaultAsync(u => u.UserName.Equals(userName) || u.Email.Equals(userName))
        //            ?? throw new NotFoundException(typeof(User).Name, userName);
        //    if (user.VerifecationCode != accountServices.HashString(code))
        //        return false;
        //    return true;
        //} 
        #endregion
    }
}
