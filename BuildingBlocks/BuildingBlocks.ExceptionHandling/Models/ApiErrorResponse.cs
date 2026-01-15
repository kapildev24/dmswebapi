namespace BuildingBlocks.ExceptionHandling.Models
{
    public sealed class ApiErrorResponse
    {
        public string TraceId { get; init; } = default!;
        public int StatusCode { get; init; }
        public string Message { get; init; } = default!;
    }
}
