using Microsoft.AspNetCore.Identity;

namespace DMS.Auth.Infrastructure.Security
{
    public sealed class PasswordHasher
    {
        private readonly PasswordHasher<User> _hasher = new();
        public string Hash(string password)
        {
            return _hasher.HashPassword(null!, password);
        }

        public bool Verify(string hashedPassword, string providedPassword)
        {
            var result = _hasher.VerifyHashedPassword(null!, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
