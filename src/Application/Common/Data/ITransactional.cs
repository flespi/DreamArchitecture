namespace CleanArchitecture.Application.Common.Data;

/// <summary>
/// Defines a contract for components that support transactional operations.
/// </summary>
public interface ITransactional
{
    /// <summary>
    /// Begins a new transaction scope.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an <see cref="ITransaction"/> that controls the lifetime
    /// of the transaction.
    /// </returns>
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}
