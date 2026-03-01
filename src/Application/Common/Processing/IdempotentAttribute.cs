namespace CleanArchitecture.Application.Common.Processing;

/// <summary>
/// Specifies the class this attribute is applied to requires idempotency.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class IdempotentAttribute : Attribute
{
}
