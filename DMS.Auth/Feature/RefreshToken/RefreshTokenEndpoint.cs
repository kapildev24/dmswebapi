namespace DMS.Auth.Feature.RefreshToken
{
    [ApiController]
    [Route("api/auth/refresh")]
    public class RefreshTokenEndpoint : ControllerBase
    {
        private readonly RefreshTokenHandler _handler;

        public RefreshTokenEndpoint(RefreshTokenHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh(
            [FromBody] RefreshTokenRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var response = await _handler.HandleAsync(request, cancellationToken);
                return Ok(response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid refresh token." });
            }
        }
    }
}
