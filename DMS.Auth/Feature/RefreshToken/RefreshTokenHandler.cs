namespace DMS.Auth.Feature.RefreshToken
{
    public sealed class RefreshTokenHandler
    {
        private readonly AuthDbContext _authDb;
        private readonly JwtTokenService _jwtTokenService;
        private readonly IConfiguration _configuration;

        public RefreshTokenHandler(
            AuthDbContext authDb,
            JwtTokenService jwtTokenService,
            IConfiguration configuration
            )
        {
            _authDb = authDb;
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
        }

        public async Task<RefreshTokenResponse> HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var existingToken = await _authDb.RefreshTokens
            .SingleOrDefaultAsync(
                x => x.Token == request.RefreshToken,
                cancellationToken);

            if (existingToken is null || !existingToken.IsActive)
                throw new UnauthorizedAccessException("Invalid refresh token.");

            var user = await _authDb.Users
                .SingleAsync(x => x.Id == existingToken.UserId, cancellationToken);

            // 🔥 ROTATION: revoke old token
            existingToken.Revoke();

            var newRefreshTokenValue = _jwtTokenService.GenerateRefreshToken();

            var refreshTokenDays =
                int.Parse(_configuration["Jwt:RefreshTokenDays"]!);

            var newRefreshToken = new Entities.RefreshToken(
                user.Id,
                newRefreshTokenValue,
                DateTime.UtcNow.AddDays(refreshTokenDays));

            _authDb.RefreshTokens.Add(newRefreshToken);

            await _authDb.SaveChangesAsync(cancellationToken);

            var accessToken = _jwtTokenService.GenerateAccessToken(user);

            return new RefreshTokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshTokenValue,
                ExpiresInSeconds =
                    int.Parse(_configuration["Jwt:AccessTokenMinutes"]!) * 60
            };
        }
    }
}
