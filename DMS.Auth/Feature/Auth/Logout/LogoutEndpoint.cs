namespace DMS.Auth.Feature.Auth.Logout
{
    [ApiController]
    [Route("api/auth/logout")]
    public sealed class LogoutEndpoint : ControllerBase
    {
        private readonly LogoutHandler _handler;

        public LogoutEndpoint(LogoutHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        [ProducesResponseType(typeof(LogoutResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<LogoutResponse>> LogoutAsync(
            [FromBody] LogoutRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _handler.HandleAsync(request, cancellationToken);
            return Ok(response);
        }
    }
}
