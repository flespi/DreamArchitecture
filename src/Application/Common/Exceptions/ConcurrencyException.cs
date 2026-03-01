using CleanArchitecture.Application.Common.Processing;
using CleanArchitecture.Domain.Types;

namespace CleanArchitecture.Application.Common.Exceptions;

public class ConcurrencyException : Exception
{
    private const string Msg = "Data may have been modified or deleted since entities were loaded.";

    public ConcurrencyException()
        : base(Msg)
    {
    }

    public ConcurrencyException(Exception innerException)
    : base(Msg, innerException)
    {
    }

    public static void ThrowIfInvalid(Condition? condition, Hex concurrencyToken)
    {
        if (condition is not null)
        {
            if (!condition.IsValid(concurrencyToken))
            {
                throw new ConcurrencyException();
            }
        }
    }
}
