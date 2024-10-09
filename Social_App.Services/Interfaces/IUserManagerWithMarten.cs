
namespace Social_App.Services.Interfaces
{
    public interface IUserManagerWithMarten
    {
        Task<bool> RegisterAccount(User user);
        Task<bool> VerifyAccount(string username, string verifecationCode);

        Task<TokenModel> LoginWithEmailOrUserName(string userName, string password);
        Task<TokenModel> RefreshTheToken(string accessToken, string refreshToken);
        Task<bool> RevokeToken(string refreshToken);

        Task<User> FindUserByEmailOrUserName(string userName);

        Task<bool> IsUserActive(string userName);
        Task<bool> DoesUserNameExists(string userName);
        // <summary>
        // check if the email is exist for another user
        // </summary>
        // <param name="email">user's email </param>
        Task<bool> DoesEmailExists(string email);

        Task<bool> RequestForResetPassword(string email);
        Task<bool> VerifyChangingPasswordCodeToken(string email, string code);
        Task<bool> ChangePassword(string email, string newPassword, string code);
    }
}
