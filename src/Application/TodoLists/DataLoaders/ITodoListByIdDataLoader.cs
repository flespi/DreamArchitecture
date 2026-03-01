using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.TodoLists.DataLoaders;

public interface ITodoListByIdDataLoader : IDataLoader<Guid, TodoList>
{
}
