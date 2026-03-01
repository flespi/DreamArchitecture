using CleanArchitecture.Application.Common.Data;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.TodoLists.Queries.GetTodos;

[Authorize]
public record GetTodosQuery : IRequest<Page<TodoList>>
{
    public PagingArguments PagingArgs { get; init; }
    
    public QueryContext<TodoList>? Specification {  get; init; }
}

public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, Page<TodoList>>
{
    private readonly IUnitOfWork _uow;

    public GetTodosQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Page<TodoList>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        var specification = request.Specification;

        if (specification is not null)
        {
            (var selector, var predicate, var sorting) = specification;

            sorting ??= SortDefinition<TodoList>.Empty;
            sorting = sorting.WithDefault(e => e.Id);

            specification = new QueryContext<TodoList>(selector, predicate, sorting);
        }

        return await _uow.TodoList.GetPageAsync(specification, request.PagingArgs, cancellationToken);
    }
}
