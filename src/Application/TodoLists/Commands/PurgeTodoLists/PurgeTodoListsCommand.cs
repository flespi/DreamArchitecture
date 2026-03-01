using CleanArchitecture.Application.Common.Processing;
using CleanArchitecture.Application.Common.Data;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.TodoLists.Commands.PurgeTodoLists;

[Authorize(Roles = Roles.Administrator)]
[Authorize(Policy = Policies.CanPurge)]
[Transactional]
public record PurgeTodoListsCommand : IRequest;

public class PurgeTodoListsCommandHandler : IRequestHandler<PurgeTodoListsCommand>
{
    private readonly IUnitOfWork _uow;

    public PurgeTodoListsCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(PurgeTodoListsCommand request, CancellationToken cancellationToken)
    {
        var specification = QueryContext<TodoList>.Empty;

        await _uow.TodoList.DeleteRangeAsync(specification, cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);
    }
}
