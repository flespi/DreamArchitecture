using CleanArchitecture.Domain.Entities;
using EntityStitching;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data.Aggregates;

public static class TodoListAggregate
{
    public static IAggregateBoundary<TodoList> Boundary { get; } = new AggregateBoundary<TodoList>()
        .Include(x => x.Items);
}
