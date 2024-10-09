using Marten;
using Social_App.API.CQRSConfigurations;
using Social_App.API.Models.Identity;

namespace Social_App.API.Features.Users.CheckEmailAvailable
{
    public class CheckEmailAvailableHandler
        (IDocumentSession session)
        : IQueryHandler<CheckEmailAvailableRequest, CheckEmailAvailableResponse>
    {
        public async Task<CheckEmailAvailableResponse> Handle(CheckEmailAvailableRequest request, CancellationToken cancellationToken)
        {
            var result = await DoesEmailExists(request.Email);
            return new CheckEmailAvailableResponse { Available = !result };
        }

        private async Task<bool> DoesEmailExists(string email)
        {
            return await session.Query<User>()
                   .Where(u => u.Email.Equals(email))
                   .AnyAsync();
        }
    }
}
