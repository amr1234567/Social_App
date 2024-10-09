using Marten;
using Social_App.API.CQRSConfigurations;
using Social_App.API.Interfaces;
using Social_App.API.Models.Exceptions;
using Social_App.API.Models.Identity;

namespace Social_App.API.Features.Users.ChangePassword
{
    public class ChangePasswordHandler
        (IDocumentSession session, IAccountServices accountServices)
        : ICommandHandler<ChangePasswordRequest, ChangePasswordResponse>
    {
        public async Task<ChangePasswordResponse> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await ChangePassword(request.UserName, request.NewPassword, request.VerifecationCode);
            return new ChangePasswordResponse { Success = result };
        }

        private async Task<bool> ChangePassword(string email, string newPassword, string code)
        {
            var user = await session.Query<User>()
                    .FirstOrDefaultAsync(u => u.UserName.Equals(email) || u.Email.Equals(email))
                    ?? throw new NotFoundException(typeof(User).Name, email);
            if (user.VerifecationCode != accountServices.HashString(code))
                return false;
            var salt = accountServices.CreateSalt();
            user.Password = accountServices.HashPasswordWithSalt(salt, newPassword);
            user.Salt = salt;
            session.Update(user);
            await session.SaveChangesAsync();
            return true;
        }

    }
}
