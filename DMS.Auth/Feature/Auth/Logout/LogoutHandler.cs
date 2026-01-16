namespace DMS.Auth.Feature.Auth.Logout
{
    public sealed class LogoutHandler
    {
        private readonly AuthDbContext _authDb;
        private readonly ILogger<LogoutHandler> _logger;

        public LogoutHandler(
            AuthDbContext authDb,
            ILogger<LogoutHandler> logger
            )
        {
            _authDb = authDb;
            _logger = logger;
        }

        public async Task<LogoutResponse> HandleAsync(LogoutRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                _logger.LogWarning("Logout attempt with empty refresh token");
                return new LogoutResponse { Success = true };
            }

            var token = await _authDb.RefreshTokens
                .FirstOrDefaultAsync(
                    t => t.Token == request.RefreshToken && t.RevokedAtUtc == null,
                    cancellationToken);

            if (token == null)
            {
                _logger.LogInformation("Logout: Token not found or already revoked");
                return new LogoutResponse { Success = true };
            }

            token.Revoke();
            await _authDb.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User logged out successfully. UserId: {UserId}", token.UserId);

            return new LogoutResponse { Success = true };
        }
    }
}
