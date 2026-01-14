namespace DMS.Auth.Feature.Auth.Login
{
    [ApiController]
    [Route("api/auth/login")]
    public class LoginEndpoint : ControllerBase
    {
        private readonly LoginHandler _handler;

        public LoginEndpoint(LoginHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponse>> LoginAsync(
            [FromBody] LoginRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var response = await _handler.HandleAsync(request, cancellationToken);
                return Ok(response);
            }
            catch (UnauthorizedAccessException)
            {
                // Prevent user enumeration
                return Unauthorized(new { message = "Invalid credentials." });
            }
        }
    }
}
