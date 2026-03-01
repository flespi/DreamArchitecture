namespace CleanArchitecture.Application.TodoLists.Commands.Shared;

public record CreateTodoListData
{
    public required string Title { get; init; }

    public IList<AddTodoListItemData>? Items { get; init; }
}
