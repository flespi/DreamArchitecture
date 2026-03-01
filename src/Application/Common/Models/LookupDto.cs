namespace CleanArchitecture.Application.Common.Models;

public class LookupDto<T>
{
    public T? Id { get; init; }

    public string? Title { get; init; }
}
