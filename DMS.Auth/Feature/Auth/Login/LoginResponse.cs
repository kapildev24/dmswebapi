namespace DMS.Auth.Feature.Auth.Login
{
    public sealed record LoginResponse
    {
        public string AccessToken { get; init; } = default!;
        public string RefreshToken { get; init; } = default!;
        public int ExpiresInSeconds { get; init; }
    }
}
