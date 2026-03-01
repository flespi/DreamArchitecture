using CleanArchitecture.Application.Common.Processing;
using CleanArchitecture.Application.Common.Data;
using CleanArchitecture.Application.TodoLists.Commands.Shared;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;

[Idempotent]
[Transactional]
public record CreateTodoListCommand : IRequest<TodoList>
{
    public required CreateTodoListData Data { get; init; }
}

public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, TodoList>
{
    private readonly IUnitOfWork _uow;

    public CreateTodoListCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<TodoList> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoList();

        entity.Title = request.Data.Title;

        if (request.Data.Items is not null)
        {
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

        await _uow.TodoList.AddAsync(entity, cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
