using CleanArchitecture.Application.Common.Processing;
using CleanArchitecture.Application.Common.Data;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.TodoLists.Commands.Shared;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.TodoLists.Commands.UpdateTodoList;

[Transactional]
public record UpdateTodoListCommand : IRequest<TodoList>
{
    public required Guid Id { get; init; }

    public required UpdateTodoListData Data { get; init; }

    public Condition? Condition { get; init; }
}

public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand, TodoList>
{
    private readonly IUnitOfWork _uow;

    public UpdateTodoListCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<TodoList> Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _uow.TodoList.GetByIdAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        ConcurrencyException.ThrowIfInvalid(request.Condition, entity.ConcurrencyToken);

        if (request.Data.Title.HasValue)
        {
            entity.Title = request.Data.Title;
        }

        if (request.Data.Items is not null)
        {
            entity.Items.Clear();

            foreach (var item in request.Data.Items)
            {
                var entityItem = new TodoItem
                {
                    Title = item.Title,
                    Note = item.Note,
                    Priority = item.Priority,
                    Done = false
                };

                entity.Items.Add(entityItem);
            }
        }

        await _uow.TodoList.UpdateAsync(entity, cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
