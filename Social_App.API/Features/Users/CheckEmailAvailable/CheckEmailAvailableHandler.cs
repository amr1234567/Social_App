
namespace Social_App.API.Features.Users.CheckEmailAvailable
{
    public class CheckEmailAvailableHandler
        (ApplicationContext context)
        : IQueryHandler<CheckEmailAvailableRequest, CheckEmailAvailableResponse>
    {
        public async Task<CheckEmailAvailableResponse> Handle(CheckEmailAvailableRequest request, CancellationToken cancellationToken)
        {
            var result = await DoesEmailExists(request.Email);
            return new CheckEmailAvailableResponse { Available = !result };
        }

        private async Task<bool> DoesEmailExists(string email)
        {
            return await context.Users.Where(u=> u.Email == email).AnyAsync();
        }

        #region Marten function with (IDocumentSession)
        //private async Task<bool> DoesEmailExists(string email)
        //{
        //    return await session.Query<User>()
        //           .Where(u => u.Email.Equals(email))
        //           .AnyAsync();
        //} 
        #endregion
    }
}
