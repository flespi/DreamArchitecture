using CleanArchitecture.Application.Common.Processing;
using CleanArchitecture.Application.Common.Data;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoLists.Commands.Shared;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.TodoLists.Commands.EditTodoListItem;

[Transactional]
public record EditTodoListItemCommand : IRequest<TodoItem>
{
    public required Guid ListId { get; init; }

    public required Guid ItemId { get; init; }

    public required EditTodoListItemData Data { get; init; }

    public Condition? Condition { get; init; }
}

public class EditTodoListItemCommandHandler : IRequestHandler<EditTodoListItemCommand, TodoItem>
{
    private readonly IUnitOfWork _uow;

    public EditTodoListItemCommandHandler(IUnitOfWork uow)
    {
       _uow = uow;
    }

    public async Task<TodoItem> Handle(EditTodoListItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _uow.TodoList.GetByIdAsync(request.ListId, cancellationToken);

        Guard.Against.NotFound(request.ListId, entity);

        ConcurrencyException.ThrowIfInvalid(request.Condition, entity.ConcurrencyToken);

        var item = entity.Items.FirstOrDefault(x => x.Id == request.ItemId);

        Guard.Against.NotFound(request.ItemId, item);

        if (request.Data.Title.HasValue)
        {
            item.Title = request.Data.Title;
        }

        if (request.Data.Note.HasValue)
        {
            item.Note = request.Data.Note;
        }

        if (request.Data.Priority.HasValue)
        {
            if (request.Data.Priority.Value.HasValue)
            {
                item.Priority = request.Data.Priority.Value.Value;
            }
        }

        if (request.Data.Done.HasValue)
        {
            if (request.Data.Done.Value.HasValue)
            {
                item.Done = request.Data.Done.Value.Value;
            }
        }

        await _uow.TodoList.UpdateAsync(entity, cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);

        return item;
    }
}
