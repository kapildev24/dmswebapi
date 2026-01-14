namespace DMS.Auth.Entities
{
    public sealed class RefreshToken
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid UserId { get; private set; }
        public string Token { get; private set; } = default!;

        public DateTime ExpiresAtUtc { get; private set; }
        public DateTime? RevokedAtUtc { get; private set; }

        public bool IsActive =>
            RevokedAtUtc is null && DateTime.UtcNow < ExpiresAtUtc;

        private RefreshToken() { }

        public RefreshToken(Guid userId, string token, DateTime expiresAtUtc)
        {
            UserId = userId;
            Token = token;
            ExpiresAtUtc = expiresAtUtc;
        }

        public void Revoke()
        {
            RevokedAtUtc = DateTime.UtcNow;
        }
    }
}
