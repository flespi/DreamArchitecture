namespace CleanArchitecture.Application.Common.Data;

/// <summary>
/// Specifies the class this attribute is applied to requires a transactional scope.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class TransactionalAttribute : Attribute
{
}
