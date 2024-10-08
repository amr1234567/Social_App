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
        Task<bool> RegisterAccount(User user);
        Task<bool> VerifyAccount(string username, string verifecationCode);

        Task<TokenModel> LoginWithEmailOrUserName(string userName, string password);

        Task<User> FindUserByEmailOrUserName(string userName);

        Task<bool> IsUserActive(string userName);
        Task<bool> UserNameExists(string userName);
        Task<bool> EmailExists(string email);
    }
}
