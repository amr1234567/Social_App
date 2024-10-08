using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Social_App.Services.Interfaces;
using System.Security.Cryptography;

namespace Social_App.Services.IdentityServices
{
    public class AccountServices : IAccountServices
    {
        public string CreateSalt()
        {
            byte[] saltBytes = new byte[128 / 8]; // 128 bits = 16 bytes
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public string HashPasswordWithSalt(string salt, string password)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            // Derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
