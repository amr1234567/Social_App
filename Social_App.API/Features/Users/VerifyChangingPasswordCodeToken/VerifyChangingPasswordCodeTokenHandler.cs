using Marten;
using Social_App.API.CQRSConfigurations;
using Social_App.API.Interfaces;
using Social_App.API.Models.Exceptions;
using Social_App.API.Models.Identity;

namespace Social_App.API.Features.Users.VerifyChangingPasswordCodeToken
{
    public class VerifyChangingPasswordCodeTokenHandler
        (IDocumentSession session,IAccountServices accountServices)
        : IQueryHandler<VerifyChangingPasswordCodeTokenRequest, VerifyChangingPasswordCodeTokenResponse>
    {
        public async Task<VerifyChangingPasswordCodeTokenResponse> Handle(VerifyChangingPasswordCodeTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await VerifyChangingPasswordCodeToken(request.UserName, request.VerifecationCode);
            return new VerifyChangingPasswordCodeTokenResponse { Authorized = result };
        }

        public async Task<bool> VerifyChangingPasswordCodeToken(string userName, string code)
        {
            var user = await session.Query<User>()
                    .FirstOrDefaultAsync(u => u.UserName.Equals(userName) || u.Email.Equals(userName))
                    ?? throw new NotFoundException(typeof(User).Name, userName);
            if (user.VerifecationCode != accountServices.HashString(code))
                return false;
            return true;
        }
    }
}
