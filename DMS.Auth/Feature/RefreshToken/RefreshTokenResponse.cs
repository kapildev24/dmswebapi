namespace DMS.Auth.Feature.RefreshToken
{
    public sealed record RefreshTokenResponse
    {
        public string AccessToken { get; init; } = default!;
        public string RefreshToken { get; init; } = default!;
        public int ExpiresInSeconds { get; init; }
    }
}
