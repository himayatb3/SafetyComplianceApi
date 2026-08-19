using SafetyCompliance.Application.Abstraction.ISecurity;
using System.Security.Cryptography;

namespace SafetyCompliance.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        public string GenerateSalt()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(SaltSize));
        }

        public string HashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                password,
                saltBytes,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize);

            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string password, string salt, string passwordHash)
        {
            var computedHash = HashPassword(password, salt);
            var computedHashBytes = Convert.FromBase64String(computedHash);
            var storedHashBytes = Convert.FromBase64String(passwordHash);

            return CryptographicOperations.FixedTimeEquals(computedHashBytes, storedHashBytes);
        }
    }
}
