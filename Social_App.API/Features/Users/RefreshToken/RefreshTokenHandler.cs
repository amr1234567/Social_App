
namespace Social_App.API.Features.Users.RefreshToken
{
    public class RefreshTokenHandler
        (ApplicationContext context, IAccountServices accountServices)
        : ICommandHandler<RefreshTokenRequest, RefreshTokenResponse>
    {
        public async Task<RefreshTokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await RefreshTheToken(request.AccessToken, request.RefreshToken);
            return new RefreshTokenResponse
            {
                Success = result is not null,
                TokenModel = result
            };
        }

        private async Task<TokenModel> RefreshTheToken(string accessToken, string refreshToken)
        {
            var user = await context.Users
               .FirstOrDefaultAsync(u => !string.IsNullOrEmpty(u.RefreshToken) && u.RefreshToken.Equals(refreshToken))
               ?? throw new NotFoundException($"User with refresh Token '{refreshToken}' not found");

            var newRefreshToken = accountServices.CreateRefreshToken();
            user.RefreshToken = newRefreshToken;
            var pricples = accountServices.GetPrincipalFromToken(accessToken);
            var newToken = accountServices.GenerateTokenFromPrincipal(pricples);

            context.Users.Update(user);
            await context.SaveChangesAsync();

            return new TokenModel
            {
                AccessToken = newToken,
                RefreshToken = newRefreshToken,
            };
        }

        #region Saving data with marten
        //private async Task<TokenModel> RefreshTheToken(string accessToken, string refreshToken)
        //{
        //    var user = await session.Query<User>()
        //       .FirstOrDefaultAsync(u => !string.IsNullOrEmpty(u.RefreshToken) && u.RefreshToken.Equals(refreshToken))
        //       ?? throw new NotFoundException($"User with refresh Token '{refreshToken}' not found");

        //    var newRefreshToken = accountServices.CreateRefreshToken();
        //    user.RefreshToken = newRefreshToken;
        //    var pricples = accountServices.GetPrincipalFromToken(accessToken);
        //    var newToken = accountServices.GenerateTokenFromPrincipal(pricples);

        //    session.Update(user);
        //    await session.SaveChangesAsync();

        //    return new TokenModel
        //    {
        //        AccessToken = newToken,
        //        RefreshToken = newRefreshToken,
        //    };
    #endregion
    }

}
