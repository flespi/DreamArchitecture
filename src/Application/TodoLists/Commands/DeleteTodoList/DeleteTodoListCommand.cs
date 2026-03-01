using CleanArchitecture.Application.Common.Processing;
using CleanArchitecture.Application.Common.Data;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.TodoLists.Commands.DeleteTodoList;

[Transactional]
public record DeleteTodoListCommand : IRequest<TodoList>
{
    public required Guid Id { get; init; }
}

public class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand, TodoList>
{
    private readonly IUnitOfWork _uow;

    public DeleteTodoListCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<TodoList> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _uow.TodoList.GetByIdAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        await _uow.TodoList.DeleteAsync(entity, cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
