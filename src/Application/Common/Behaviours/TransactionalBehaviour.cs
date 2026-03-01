using System.Reflection;
using CleanArchitecture.Application.Common.Data;

namespace CleanArchitecture.Application.Common.Behaviours;

public class TransactionalBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWork _uow;

    public TransactionalBehaviour(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var transactionalAttributes = request.GetType().GetCustomAttributes<TransactionalAttribute>();

        if (transactionalAttributes.Any())
        {
            await using var trasaction = await _uow.BeginTransactionAsync(cancellationToken);

            var result = await next();
            await trasaction.CommitAsync(cancellationToken);
            return result;
        }

        return await next();
    }
}
