using CleanArchitecture.Application.Common.Processing;

namespace CleanArchitecture.Application.FunctionalTests.Internal.Services;

public class IdempotentRequest : IIdempotentRequest
{
    public string? IdempotencyKey { get; } = Guid.NewGuid().ToString();
}
