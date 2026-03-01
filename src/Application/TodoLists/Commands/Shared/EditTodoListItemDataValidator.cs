using CleanArchitecture.Application.TodoLists.Validators;

namespace CleanArchitecture.Application.TodoLists.Commands.Shared;

public class EditTodoListItemDataValidator : AbstractValidator<EditTodoListItemData>
{
    public EditTodoListItemDataValidator()
    {
        RuleFor(v => v.Title.Value)
            .SetValidator(TodoItemRuleSet.Title)
            .When(v => v.Title.HasValue);
    }
}
