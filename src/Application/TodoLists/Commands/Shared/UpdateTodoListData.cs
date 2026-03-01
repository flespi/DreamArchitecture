namespace CleanArchitecture.Application.TodoLists.Commands.Shared;

public record UpdateTodoListData
{
    public Optional<string> Title { get; init; }

    public IList<AddTodoListItemData>? Items { get; init; }
}
