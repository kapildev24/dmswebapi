namespace DMS.Auth.Feature.Auth.Login
{
    public sealed record LoginRequest
    {
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
    }
}
