using CleanArchitecture.Application.Common.Processing;

namespace CleanArchitecture.Application.FunctionalTests;

public class IdempotentRequest : IIdempotentRequest
{
    public string? IdempotencyKey { get; } = Guid.NewGuid().ToString();
}
