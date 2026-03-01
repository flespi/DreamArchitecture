using CleanArchitecture.Application.TodoLists.Commands.Shared;

namespace CleanArchitecture.Application.TodoLists.Commands.EditTodoListItem;

public class EditTodoListItemCommandValidator : AbstractValidator<EditTodoListItemCommand>
{
    public EditTodoListItemCommandValidator()
    {
        RuleFor(v => v.Data)
            .NotNull()
            .SetValidator(new EditTodoListItemDataValidator());
    }
}
