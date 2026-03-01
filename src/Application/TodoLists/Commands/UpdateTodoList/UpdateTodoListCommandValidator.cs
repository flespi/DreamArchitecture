using CleanArchitecture.Application.Common.Data;
using CleanArchitecture.Application.Resources;
using CleanArchitecture.Application.TodoLists.Commands.Shared;

namespace CleanArchitecture.Application.TodoLists.Commands.UpdateTodoList;

public class UpdateTodoListCommandValidator : AbstractValidator<UpdateTodoListCommand>
{
    public UpdateTodoListCommandValidator(
        IUnitOfWork uow,
        IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(v => v.Data)
            .NotNull()
            .SetValidator(new UpdateTodoListDataValidator());

        RuleFor(v => v.Data.Title.Value)
            .MustAsync(BeUniqueTitle)
                .WithMessage(localizer["Unique_Field"])
                .WithName("Title")
                .WithErrorCode("Unique");

        async Task<bool> BeUniqueTitle(UpdateTodoListCommand request, string? title, CancellationToken cancellationToken)
        {
            return !await uow.TodoList.AnyAsync(
                t => t.Title == title && t.Id != request.Id,
                cancellationToken);
        }
    }
}
