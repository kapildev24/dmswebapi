namespace DMS.Auth.Feature.Auth.Register
{
    [ApiController]
    [Route("api/auth/register")]
    public sealed class RegisterEndpoint : ControllerBase
    {
        private readonly RegisterHandler _handler;

        public RegisterEndpoint(RegisterHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<RegisterResponse>> RegisterAsync(
            [FromBody] RegisterRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _handler.HandleAsync(request, cancellationToken);
            return Ok(response);
        }
    }
}
