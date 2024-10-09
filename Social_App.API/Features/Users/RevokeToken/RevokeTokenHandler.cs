using Marten;
using Social_App.API.CQRSConfigurations;
using Social_App.API.Models.Exceptions;
using Social_App.API.Models.Identity;

namespace Social_App.API.Features.Users.RevokeToken
{
    public class RevokeTokenHandler
        (IDocumentSession session)
        : ICommandHandler<RevokeTokenRequest, RevokeTokenResponse>
    {
        public async Task<RevokeTokenResponse> Handle(RevokeTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await RevokeToken(request.RefreshToken);
            return new RevokeTokenResponse(result);
        }

        private async Task<bool> RevokeToken(string refreshToken)
        {
            var user = await session.Query<User>()
                .FirstOrDefaultAsync(u => !string.IsNullOrEmpty(u.RefreshToken) && u.RefreshToken.Equals(refreshToken))
                ?? throw new NotFoundException($"User with refresh Token '{refreshToken}' not found");
            user.RefreshToken = null;
            session.Update(user);
            await session.SaveChangesAsync();

            return true;
        }

    }
}
