using CleanArchitecture.Application.TodoLists.DataLoaders;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Graph.Resolvers;

public class TodoListResolvers
{
    public async Task<IEnumerable<TodoItem>?> GetTodoItemAsync(
        [Parent] TodoList todoList,
        ITodoItemsByListIdDataLoader itemsBylistId,
        QueryContext<TodoItem>? query,
        CancellationToken cancellationToken)
    {
        return await itemsBylistId.With(query).LoadAsync(todoList.Id, cancellationToken);
    }
}
