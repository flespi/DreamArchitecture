namespace CleanArchitecture.Application.Common.Processing;

public interface IIdempotentRequest
{
    public string? IdempotencyKey { get; }
}
