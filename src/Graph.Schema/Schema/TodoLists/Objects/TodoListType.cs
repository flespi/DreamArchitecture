using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Graph.Resolvers;
using CleanArchitecture.Graph.Schema.Shared.Scalars;

namespace CleanArchitecture.Graph.Schema.TodoLists.Objects;

public class TodoListType : ObjectType<TodoList>
{
    protected override void Configure(IObjectTypeDescriptor<TodoList> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(x => x.Id);

        descriptor.Field(x => x.Title);

        descriptor.Field(x => x.Colour)
            .Type<ColourType>();

        descriptor.Field(x => x.Items)
            .ResolveWith<TodoListResolvers>(x => x.GetTodoItemAsync(default!, default!, default!, default));
    }
}
