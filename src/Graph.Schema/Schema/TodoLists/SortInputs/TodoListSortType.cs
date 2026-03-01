using CleanArchitecture.Domain.Entities;
using HotChocolate.Data.Sorting;

namespace CleanArchitecture.Graph.Schema.TodoLists.SortInputs;

public class TodoListSortType : SortInputType<TodoList>
{
    protected override void Configure(ISortInputTypeDescriptor<TodoList> descriptor)
    {
        descriptor.Field(t => t.Title);
    }
}
