using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.TodoLists.Commands.Shared;

public record EditTodoListItemData
{
    public Optional<string?> Title { get; init; }

    public Optional<string?> Note { get; init; }

    public Optional<PriorityLevel?> Priority { get; init; }

    public Optional<bool?> Done { get; init; }
}
