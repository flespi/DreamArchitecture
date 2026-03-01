using CleanArchitecture.Application.TodoLists.DataLoaders;
using CleanArchitecture.Graph.DataLoaders;
using HotChocolate.Execution.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IRequestExecutorBuilder AddDataLoaders(this IRequestExecutorBuilder builder)
    {
        return builder
            .AddDataLoader<ITodoItemsByListIdDataLoader, TodoItemsByListIdDataLoader>()
            .AddDataLoader<ITodoListByIdDataLoader, TodoListByIdDataLoader>();
    }
}
