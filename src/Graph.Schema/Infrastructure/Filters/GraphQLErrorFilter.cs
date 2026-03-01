using CleanArchitecture.Application.Common.Exceptions;

namespace CleanArchitecture.Graph.Infrastructure.Filters;

public class GraphQLErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        if (error.Exception is null)
        {
            return error;
        }

        var ex = error.Exception;

        return ex switch
        {
            ValidationException ve => HandleValidation(ve, error),
            NotFoundException nf => HandleNotFound(nf, error),
            UnauthorizedAccessException ua => HandleUnauthorized(ua, error),
            ForbiddenAccessException fa => HandleForbidden(fa, error),
            ConcurrencyException ce => HandleConcurrency(ce, error),
            _ => HandleUnexpected(ex, error)
        };
    }

    private IError HandleValidation(ValidationException ve, IError error)
    {
        var errors = ve.Errors.ToDictionary(
            kvp => kvp.Key,
            kvp => (object?)kvp.Value
        );

        return error
            .WithMessage("Validation failed for one or more fields.")
            .WithCode("VALIDATION_ERROR")
            .SetExtension("validationErrors", errors);
    }

    private IError HandleNotFound(NotFoundException nf, IError error)
    {
        return error
            .WithMessage(nf.Message)
            .WithCode("NOT_FOUND");
    }

    private IError HandleUnauthorized(UnauthorizedAccessException ua, IError error)
    {
        return error
            .WithMessage("Not authenticated.")
            .WithCode("UNAUTHORIZED");
    }

    private IError HandleForbidden(ForbiddenAccessException fa, IError error)
    {
        return error
            .WithMessage("You do not have permission to perform this action.")
            .WithCode("FORBIDDEN");
    }

    private IError HandleConcurrency(ConcurrencyException ce, IError error)
    {
        return error
            .WithMessage("The resource was modified by another process.")
            .WithCode("CONCURRENCY_ERROR");
    }

    private IError HandleUnexpected(Exception ex, IError error)
    {
        return error
            .WithMessage("An unexpected error occurred.")
            .WithCode("INTERNAL_ERROR");
    }
}
