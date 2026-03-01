using CleanArchitecture.Application.TodoLists.Validators;

namespace CleanArchitecture.Application.TodoLists.Commands.Shared;

public class CreateTodoListDataValidator : AbstractValidator<CreateTodoListData>
{
    public CreateTodoListDataValidator()
    {
        RuleFor(v => v.Title)
            .SetValidator(TodoListRuleSet.Title);

        RuleForEach(v => v.Items)
            .SetValidator(new AddTodoListItemDataValidator())
            .When(v => v is not null);
    }
}
