namespace CleanArchitecture.Application.TodoLists.Validators;

public static class TodoListRuleSet
{
    public static InlineValidator<string?> Title { get; } = [];

    static TodoListRuleSet()
    {
        Title
            .RuleFor(x => x)
            .NotEmpty()
            .MaximumLength(200);
    }
}
