using Marten;
using Microsoft.Extensions.Logging;
using Social_App.API.CQRSConfigurations;
using Social_App.API.Interfaces;
using Social_App.API.Models.Exceptions;
using Social_App.API.Models.Identity;

namespace Social_App.API.Features.Users.RequestForResetPassword
{
    public class RequestForResetPasswordHandler 
        (IDocumentSession session,IAccountServices accountServices, ILogger<RequestForResetPasswordHandler> logger)
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
            var user = await session.Query<User>()
                    .FirstOrDefaultAsync(u => u.UserName.Equals(userName) || u.Email.Equals(userName))
                    ?? throw new NotFoundException(typeof(User).Name, userName);
            user.NumberOfTries += 1;
            if (user.NumberOfTries > maxNumOfTries)
            {
                user.AccountLocked = true;
                session.Update(user);
                await session.SaveChangesAsync();
                throw new Exception("User Rech The max number of tries");
            }

            var code = accountServices.CreateVerifecationCode(6);
            logger.LogInformation($"[Verifecation Code For Password] ==> '{code}'");
            user.VerifecationCode = accountServices.HashString(code);
            user.FinalTimeForVerify = DateTime.Now.AddMinutes(60);

            //await emailSender.SendEmailAsync(user.Email, "Verify your account", $"<h2>Your verifecation code is <strong>{code}</strong></h2>");

            session.Update(user);
            await session.SaveChangesAsync();
            return true;
        }

    }
}
