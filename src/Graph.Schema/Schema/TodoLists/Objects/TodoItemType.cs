using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Graph.Schema.TodoLists.Objects;

public class TodoItemType : ObjectType<TodoItem>
{
    protected override void Configure(IObjectTypeDescriptor<TodoItem> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(x => x.Id);

        descriptor.Field(x => x.Title);
    }
}
