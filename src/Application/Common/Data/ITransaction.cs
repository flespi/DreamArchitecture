namespace CleanArchitecture.Application.Common.Data;

/// <summary>
/// Represents an asynchronous transaction scope.
/// </summary>
public interface ITransaction : IAsyncDisposable
{
    /// <summary>
    /// Commits all changes made within the current transaction.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back all changes made within the current transaction.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    Task RollbackAsync(CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    async ValueTask IAsyncDisposable.DisposeAsync() => await RollbackAsync();
}

