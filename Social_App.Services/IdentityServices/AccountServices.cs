using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite.Algorithm;
using Social_App.Core.Helpers;
using Social_App.Services.Interfaces;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Social_App.Services.IdentityServices
{
    public class AccountServices(IOptions<JwtDetails> options)
        : IAccountServices
    {
        private readonly JwtDetails _jwt = options.Value;
        public string CreateJwtToken(List<Claim> claims)
        {
            var customClaims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.PrimarySid, Guid.NewGuid().ToString()),
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.Now.AddMinutes(_jwt.JwtExpireMinutes);

            customClaims.Add(new(ClaimTypes.Expiration, expires.ToString()));

            var token = new JwtSecurityToken(
                _jwt.JwtIssuer,
                _jwt.JwtAudience,
                claims.Union(customClaims),
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            // Convert the byte array to a Base64 string
            return Convert.ToBase64String(randomNumber);
        }

        public string CreateSalt()
        {
            byte[] saltBytes = new byte[128 / 8]; 
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public string CreateVerifecationCode(int length)
        {
            string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            if (length <= 0)
                throw new ArgumentException("Length must be a positive integer.");

            Random random = new();
            StringBuilder verificationCode = new(length);

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(Characters.Length);
                verificationCode.Append(Characters[index]);
            }

            return verificationCode.ToString();
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

        public string HashString(string input)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));

            StringBuilder builder = new();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Define the token validation parameters
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.JwtKey)),
                    ValidateIssuer = true,
                    ValidIssuer = _jwt.JwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = _jwt.JwtAudience,
                    ValidateLifetime = true, // Ensure token is not expired
                    ClockSkew = TimeSpan.Zero // No tolerance for expiration
                };

                // Validate the token and extract the ClaimsPrincipal
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                // Optional: Check if the token is a valid JWT token
                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    // Additional validation (e.g., verify signing algorithm if needed)
                    if (!jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    {
                        throw new SecurityTokenException("Invalid token algorithm");
                    }
                }

                return principal;
            }
            catch (Exception ex)
            {
                // Handle validation failure
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return null; // Return null if token is invalid
            }
        }

        public string GenerateTokenFromPrincipal(ClaimsPrincipal principal)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwt.JwtKey);

            // Extract claims from the principal
            var claims = new List<Claim>(principal.Claims);

            // Define the token descriptor with claims, expiration, signing key, etc.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwt.JwtExpireMinutes),
                Issuer = _jwt.JwtIssuer,
                Audience = _jwt.JwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Create the token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return the serialized token
            return tokenHandler.WriteToken(token);
        }

        public bool BeAValidUserNameOrEmail(string username)
        {
            var userNameRegex = new Regex("^[a-zA-Z0-9-_]+$");
            // Regular expression for email
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            return userNameRegex.IsMatch(username) || emailRegex.IsMatch(username);
        }
    }
}
