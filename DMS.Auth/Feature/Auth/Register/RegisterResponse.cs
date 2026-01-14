namespace DMS.Auth.Feature.Auth.Register
{
    public sealed record RegisterResponse
    {
        public Guid UserId { get; init; }
        public string Email { get; init; } = default!;
    }
}
