using CleanArchitecture.Application.Common.Data;
using CleanArchitecture.Application.Resources;
using CleanArchitecture.Application.TodoLists.Commands.Shared;

namespace CleanArchitecture.Application.TodoLists.Commands.CreateTodoList;

public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
{
    public CreateTodoListCommandValidator(
        IUnitOfWork uow,
        IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(v => v.Data)
            .NotNull()
            .SetValidator(new CreateTodoListDataValidator());

        RuleFor(v => v.Data.Title)
            .MustAsync(BeUniqueTitle)
                .WithMessage(localizer["Unique_Field"])
                .WithErrorCode("Unique");

        async Task<bool> BeUniqueTitle(CreateTodoListCommand request, string? title, CancellationToken cancellationToken)
        {
            return !await uow.TodoList.AnyAsync(
                t => t.Title == title,
                cancellationToken);
        }
    }
}
