using System.Linq.Expressions;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Application.TodoLists.DataLoaders;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.TodoLists.Queries.GetTodoById;

[Authorize]
public record GetTodoByIdQuery : IRequest<TodoList>
{
    public required Guid Id { get; init; }

    public Expression<Func<TodoList,TodoList>>? Selector {  get; init; }
}

public class GetTodosQueryHandler : IRequestHandler<GetTodoByIdQuery, TodoList>
{
    private readonly ITodoListByIdDataLoader _dataLoader;

    public GetTodosQueryHandler(ITodoListByIdDataLoader dataLoader)
    {
        _dataLoader = dataLoader;
    }

    public async Task<TodoList> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dataLoader.Select(request.Selector).LoadAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException(request.Id.ToString(), nameof(TodoList));
        }

        return entity;
    }
}
