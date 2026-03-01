using System.ComponentModel;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.TodoLists.Commands.Shared;

public record AddTodoListItemData
{
    public required string Title { get; init; }

    public string? Note { get; init; }

    [DefaultValue(PriorityLevel.None)]
    public Optional<PriorityLevel> Priority { get; init; }
}
