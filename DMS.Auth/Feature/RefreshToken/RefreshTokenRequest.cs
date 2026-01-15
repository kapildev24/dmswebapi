namespace DMS.Auth.Feature.RefreshToken
{
    public sealed record RefreshTokenRequest
    {
        public string RefreshToken { get; init; } = default!;
    }
}
