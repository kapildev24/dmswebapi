using BuildingBlocks.ExceptionHandling.Exceptions;
using BuildingBlocks.ExceptionHandling.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BuildingBlocks.ExceptionHandling.Middleware
{
    public sealed class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _request;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public GlobalExceptionMiddleware(
            RequestDelegate request,
            ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment environment
            )
        {
            _request = request;
            _logger = logger;
            _environment = environment;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _request(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            var traceId = context.TraceIdentifier;

            _logger.LogError(exception,
                "Unhandled exception | TraceId: {TraceId}",
                traceId);

            var statusCode = exception switch
            {
                ApiException apiEx => apiEx.StatusCode,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                InvalidOperationException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            var message = _environment.IsDevelopment()
                ? exception.Message
                : "An unexpected error occurred.";

            var response = new ApiErrorResponse
            {
                TraceId = traceId,
                StatusCode = statusCode,
                Message = message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
