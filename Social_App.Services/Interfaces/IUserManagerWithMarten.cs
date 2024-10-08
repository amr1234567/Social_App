using Social_App.Core.Helpers;
using Social_App.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_App.Services.Interfaces
{
    public interface IUserManagerWithMarten
    {
        Task<string> RegisterAccount(User user);
        Task<bool> VerifyAccount(User user, string verifecationCode);

        Task<TokenModel> LoginWithEmail(string email, string password);
        Task<TokenModel> LoginWithUserName(string userName, string password);

        Task<User> FindUserByUserName(string userName);
        Task<User> FindUserByEmail(string email);

        Task<bool> IsEmailActive(string email);
        Task<bool> IsUserNameActive(string userName);
        Task<bool> UserNameExists(string userName);
        Task<bool> EmailExists(string email);
    }
}
