namespace DMS.Auth.Entities
{
    public sealed class User
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Email { get; private set; } = default!;
        public string PasswordHash { get; private set; } = default!;

        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

        private User() { }

        public User(string email, string passwordHash)
        {
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}
