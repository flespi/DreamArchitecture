using CleanArchitecture.Application.TodoLists.Commands.Shared;

namespace CleanArchitecture.Application.TodoLists.Commands.AddTodoListItem;

public class AddTodoListItemCommandValidator : AbstractValidator<AddTodoListItemCommand>
{
    public AddTodoListItemCommandValidator()
    {
        RuleFor(v => v.Data)
            .NotNull()
            .SetValidator(new AddTodoListItemDataValidator());
    }
}
