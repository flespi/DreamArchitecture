using CleanArchitecture.Application.TodoLists.Validators;

namespace CleanArchitecture.Application.TodoLists.Commands.Shared;

public class AddTodoListItemDataValidator : AbstractValidator<AddTodoListItemData>
{
    public AddTodoListItemDataValidator()
    {
        RuleFor(v => v.Title)
            .SetValidator(TodoItemRuleSet.Title);
    }
}
