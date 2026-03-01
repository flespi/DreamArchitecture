using CleanArchitecture.Domain.Entities;
using HotChocolate.Data.Filters;

namespace CleanArchitecture.Graph.Schema.TodoLists.FilterInputs;

public class TodoListFilterType : FilterInputType<TodoList>
{
    protected override void Configure(IFilterInputTypeDescriptor<TodoList> descriptor)
    {
        descriptor.Field(t => t.Id);
        descriptor.Field(t => t.Title);
    }
}
