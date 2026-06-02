using CleanArchitecture.Application.Common.Processing;

namespace CleanArchitecture.Graph.SchemaTests.Internal.Services;

public class IdempotentRequest : IIdempotentRequest
{
    public string? IdempotencyKey { get; } = Guid.NewGuid().ToString();
}

