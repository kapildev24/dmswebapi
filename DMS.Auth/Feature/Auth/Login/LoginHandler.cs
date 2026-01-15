namespace DMS.Auth.Feature.Auth.Login
{
    public class LoginHandler
    {
        private readonly AuthDbContext _authDb;
        private readonly PasswordHasher _hasher;
        private readonly JwtTokenService _jwtTokenService;
        private readonly IConfiguration _configuration;

        public LoginHandler(
            AuthDbContext authDb,
            PasswordHasher hasher,
            JwtTokenService jwtTokenService,
            IConfiguration configuration
            )
        {
            _authDb = authDb;
            _hasher = hasher;
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
        }
        public async Task<LoginResponse> HandleAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var user = await _authDb.Users
                .SingleOrDefaultAsync(x =>x.Email == email, cancellationToken);
            if (user is null)
                throw new UnauthorizedAccessException("Invalid credentials.");

            var isPasswordValid =
                _hasher.Verify(user.PasswordHash, request.Password);

            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Invalid credentials.");

            var accessToken = _jwtTokenService.GenerateAccessToken(user);
            var refreshTokenValue = _jwtTokenService.GenerateRefreshToken();

            var refreshTokenDays =
                int.Parse(_configuration["Jwt:RefreshTokenDays"]!);

            var refreshToken = new Entities.RefreshToken(
                user.Id,
                refreshTokenValue,
                DateTime.UtcNow.AddDays(refreshTokenDays));

            _authDb.RefreshTokens.Add(refreshToken);
            await _authDb.SaveChangesAsync(cancellationToken);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                ExpiresInSeconds =
                    int.Parse(_configuration["Jwt:AccessTokenMinutes"]!) * 60
            };
        }
    }
}
