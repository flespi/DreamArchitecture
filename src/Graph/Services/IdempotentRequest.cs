using CleanArchitecture.Application.Common.Processing;

namespace CleanArchitecture.Graph.Services;

public class IdempotentRequest : IIdempotentRequest
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdempotentRequest(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? IdempotencyKey => _httpContextAccessor.HttpContext?.Request.Headers["Idempotency-Key"];
}
