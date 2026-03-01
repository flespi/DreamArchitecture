using CleanArchitecture.Application.TodoLists.DataLoaders;
using CleanArchitecture.Application.TodoLists.Queries.GetTodos;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Graph.Schema.TodoLists.FilterInputs;
using CleanArchitecture.Graph.Schema.TodoLists.SortInputs;
using HotChocolate.Authorization;
using HotChocolate.Execution.Processing;
using HotChocolate.Types.Pagination;

namespace CleanArchitecture.Graph.Operations.Queries;

[Authorize]
[QueryType]
public static class TodoListQuery
{
    [NodeResolver]
    public static async Task<TodoList?> GetTodoListById(
        Guid id,
        ISelection selection,
        ITodoListByIdDataLoader dataLoader,
        CancellationToken cancellationToken)
    {
        return await dataLoader.Select(selection).LoadAsync(id, cancellationToken);
    }

    [UsePaging]
    [UseFiltering(typeof(TodoListFilterType))]
    [UseSorting(typeof(TodoListSortType))]
    public static async Task<Connection<TodoList>> GetTodoLists(
        PagingArguments pagingArgs,
        QueryContext<TodoList> query,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var request = new GetTodosQuery
        {
            PagingArgs = pagingArgs,
            Specification = query,
        };

        return await sender.Send(request, cancellationToken).ToConnectionAsync();
    }
}
