using CleanArchitecture.Application.Common.Processing;
using CleanArchitecture.Application.Common.Data;
using CleanArchitecture.Application.TodoLists.Commands.Shared;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Events;

namespace CleanArchitecture.Application.TodoLists.Commands.AddTodoListItem;

[Idempotent]
[Transactional]
public record AddTodoListItemCommand : IRequest<TodoItem>
{
    public required Guid ListId { get; init; }

    public required AddTodoListItemData Data { get; init; }
}

public class AddTodoListItemCommandHandler : IRequestHandler<AddTodoListItemCommand, TodoItem>
{
    private readonly IUnitOfWork _uow;

    public AddTodoListItemCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<TodoItem> Handle(AddTodoListItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _uow.TodoList.GetByIdAsync(request.ListId, cancellationToken);

        Guard.Against.NotFound(request.ListId, entity);

        var item = new TodoItem
        {
            ListId = request.ListId,
            Title = request.Data.Title,
            Note = request.Data.Note,
            Priority = request.Data.Priority,
            Done = false
        };

        entity.Items.Add(item);

        entity.AddDomainEvent(new TodoItemCreatedEvent(item));

        await _uow.TodoList.UpdateAsync(entity, cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);

        return item;
    }
}
