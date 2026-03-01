using CleanArchitecture.Application.Common.Processing;
using CleanArchitecture.Application.Common.Data;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Events;

namespace CleanArchitecture.Application.TodoLists.Commands.RemoveTodoListItem;

[Transactional]
public record RemoveTodoListItemCommand : IRequest<TodoItem>
{
    public required Guid ListId { get; init; }

    public required Guid ItemId { get; init; }
}

public class RemoveTodoListItemCommandHandler : IRequestHandler<RemoveTodoListItemCommand, TodoItem>
{
    private readonly IUnitOfWork _uow;

    public RemoveTodoListItemCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<TodoItem> Handle(RemoveTodoListItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _uow.TodoList.GetByIdAsync(request.ListId, cancellationToken);

        Guard.Against.NotFound(request.ListId, entity);

        var item = entity.Items.FirstOrDefault(x => x.Id == request.ItemId);

        Guard.Against.NotFound(request.ItemId, item);

        entity.Items.Remove(item);

        entity.AddDomainEvent(new TodoItemDeletedEvent(item));

        await _uow.TodoList.UpdateAsync(entity, cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);

        return item;
    }

}
