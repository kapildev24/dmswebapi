namespace DMS.Auth.Feature.Auth.Register
{
    public sealed class RegisterHandler
    {
        private readonly AuthDbContext _authDb;
        private readonly PasswordHasher _hasher;

        public RegisterHandler(AuthDbContext authDb, PasswordHasher hasher)
        {
            _authDb = authDb;
            _hasher = hasher;
        }

        public async Task<RegisterResponse> HandleAsync(RegisterRequest request, CancellationToken cancellationToken)
        {
            var email = NormalizeEmail(request.Email);
            
            var passwordHash = _hasher.Hash(request.Password);

            var user = new User(email, passwordHash);

            _authDb.Users.Add(user);

            try
            {
                await _authDb.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                // UNIQUE constraint violation (email already exists)
                throw new InvalidOperationException("Email already registered.");
            }

            return new RegisterResponse
            {
                UserId = user.Id,
                Email = user.Email
            };
        }
        private static string NormalizeEmail(string email)
        {
            return email.Trim().ToLowerInvariant();
        }
    }
}
