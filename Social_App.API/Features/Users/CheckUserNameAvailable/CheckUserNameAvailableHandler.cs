

namespace Social_App.API.Features.Users.CheckUserNameAvailable
{
    public class CheckUserNameAvailableHandler
        (ApplicationContext context)
        : IQueryHandler<CheckUserNameAvailableRequest, CheckUserNameAvailableResponse>
    {
        public async Task<CheckUserNameAvailableResponse> Handle(CheckUserNameAvailableRequest request, CancellationToken cancellationToken)
        {
            var result = await DoesUserNameExists(request.UserName);
            return new CheckUserNameAvailableResponse { Available = !result };
        }

        private async Task<bool> DoesUserNameExists(string userName)
        {
            return await context.Users
                    .Where(u => u.UserName.Equals(userName))
                    .AnyAsync();
        }

        #region Marten Function
        //private async Task<bool> DoesUserNameExists(string userName)
        //{
        //    return await session.Query<User>()
        //            .Where(u => u.UserName.Equals(userName))
        //            .AnyAsync();
        //} 
        #endregion
    }
}
