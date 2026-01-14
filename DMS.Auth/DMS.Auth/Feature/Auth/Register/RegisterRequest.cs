namespace DMS.Auth.Feature.Auth.Register
{
    public sealed record RegisterRequest
    {
        //record → immutable request DTO
        //sealed → small performance + design safety
        //init → request is set once
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
    }
}
