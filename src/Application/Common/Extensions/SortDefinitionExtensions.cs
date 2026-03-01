using System.Linq.Expressions;

namespace CleanArchitecture.Application;

public static class SortDefinitionExtensions
{
    public static SortDefinition<T> WithDefault<T, TResult>(this SortDefinition<T> sorting, Expression<Func<T, TResult>> keySelector)
        => sorting.IfEmpty(o => o.AddAscending(keySelector));
}
