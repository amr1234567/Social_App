using Marten;
using Social_App.Core.Exceptions;
using Social_App.Core.Helpers;
using Social_App.Core.Identity;
using Social_App.Services.Interfaces;

namespace Social_App.Services.IdentityServices
{
    public class UserManagerWithMarten(IDocumentSession session, IAccountServices accountServices) 
        : IUserManagerWithMarten
    {
        public Task<bool> EmailExists(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindUserByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEmailActive(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserNameActive(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<TokenModel> LoginWithEmail(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task<TokenModel> LoginWithUserName(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<string> RegisterAccount(User user)
        {
            var userExistCheck = await session.Query<User>()
                .Where(u => u.Email.Equals(user.Email) || u.UserName.Equals(user.UserName))
                .AnyAsync();
            if (userExistCheck)
                throw new BadRequestException($"User with this email or user name is already exist.");

            var salt = accountServices.CreateSalt();
            var hashedPassword = accountServices.HashPasswordWithSalt(salt, user.Password);

            user.Password = hashedPassword;
            user.Salt = salt;

            session.Store(user);
            await session.SaveChangesAsync();

            // TODO:
            // call the createVerifecationCode service here
            // Save it in the object in the attribute 
            // return the Verifecation Code 

            return "verifecation code";
        }

        public Task<bool> UserNameExists(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyAccount(User user, string verifecationCode)
        {
            throw new NotImplementedException();
        }
    }
}
