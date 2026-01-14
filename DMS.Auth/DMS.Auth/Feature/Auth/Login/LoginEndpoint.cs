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
        public async Task<ActionResult<LoginResponse>> LoginAsync(
            [FromBody] LoginRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _handler.HandleAsync(request, cancellationToken);
            return Ok(response);
        }
    }
}
