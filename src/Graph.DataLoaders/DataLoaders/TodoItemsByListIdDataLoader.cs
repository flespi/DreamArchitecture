using CleanArchitecture.Application.TodoLists.DataLoaders;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Graph.DataLoaders;

public partial class TodoItemsByListIdDataLoader : ITodoItemsByListIdDataLoader
{
    [DataLoader("TodoItemsByListId")]
    public static async Task<IReadOnlyDictionary<Guid, TodoItem[]>> FetchAsync(
        IReadOnlyList<Guid> listIds,
        QueryContext<TodoItem> query,
        ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        return await context.TodoLists
            .Where(t => listIds.Contains(t.Id))
            .SelectMany(t => t.Items)
            .GroupBy(t => t.ListId, query.Selector!)
            .ToDictionaryAsync(
                g => g.Key,
                g => g.ToArray(),
                cancellationToken
            );
    }
}
