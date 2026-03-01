namespace CleanArchitecture.Application.TodoLists.Validators;

public static class TodoItemRuleSet
{
    public static InlineValidator<string?> Title { get; } = [];

    static TodoItemRuleSet()
    {
        Title
            .RuleFor(x => x)
            .NotEmpty()
            .MaximumLength(200);
    }
}
