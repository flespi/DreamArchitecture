namespace CleanArchitecture.Application.Common.Processing;

public static class ConditionExtensions
{
    public static bool IsValid(this Condition condition, string concurrencyToken)
    {
        return string.Equals(condition.IfMatch, concurrencyToken, StringComparison.InvariantCultureIgnoreCase);
    }
}

