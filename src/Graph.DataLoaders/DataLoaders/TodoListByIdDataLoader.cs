using CleanArchitecture.Application.TodoLists.DataLoaders;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Graph.DataLoaders;

public partial class TodoListByIdDataLoader : ITodoListByIdDataLoader
{
    [DataLoader("TodoListById")]
    public static async Task<IReadOnlyDictionary<Guid, TodoList>> FetchAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<TodoList> query,
        ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        return await context.TodoLists
            .Where(t => ids.Contains(t.Id))
            .With(query)
            .ToDictionaryAsync(g => g.Id, cancellationToken);
    }
}
