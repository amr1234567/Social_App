using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Social_App.Services.Interfaces
{
    public interface IAccountServices
    {
        string HashPasswordWithSalt(string salt,string password);
        bool BeAValidUserNameOrEmail(string username);
        string CreateSalt();
        string CreateVerifecationCode(int length);
        string HashString(string input);
        string CreateJwtToken(List<Claim> claims);
        string CreateRefreshToken();
        ClaimsPrincipal GetPrincipalFromToken(string token);
        string GenerateTokenFromPrincipal(ClaimsPrincipal principal);
    }
}
