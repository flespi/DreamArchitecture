using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.TodoLists.DataLoaders;

public interface ITodoItemsByListIdDataLoader : IDataLoader<Guid, TodoItem[]>
{
}
