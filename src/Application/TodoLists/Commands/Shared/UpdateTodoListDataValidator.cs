using CleanArchitecture.Application.TodoLists.Validators;

namespace CleanArchitecture.Application.TodoLists.Commands.Shared;

public class UpdateTodoListDataValidator : AbstractValidator<UpdateTodoListData>
{
    public UpdateTodoListDataValidator()
    {
        RuleFor(v => v.Title.Value)
            .SetValidator(TodoListRuleSet.Title)
            .When(v => v.Title.HasValue);

        RuleForEach(v => v.Items)
            .SetValidator(new AddTodoListItemDataValidator())
            .When(v => v is not null);
    }
}
