namespace CleanArchitecture.Application.Common.Data;

public abstract class Transaction : ITransaction
{
    private readonly SemaphoreSlim _semaphore = new(1);

    private bool _completed = false;

    protected abstract Task ExecuteCommitAsync(CancellationToken cancellationToken = default);

    protected abstract Task ExecuteRollbackAsync(CancellationToken cancellationToken = default);

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);

        if (_completed)
        {
            throw new TransactionCompletedException();
        }

        await ExecuteCommitAsync(cancellationToken);

        _completed = true;

        _semaphore.Release();
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);

        if (_completed)
        {
            throw new TransactionCompletedException();
        }

        await ExecuteRollbackAsync(cancellationToken);

        _completed = true;

        _semaphore.Release();
    }

    public async ValueTask DisposeAsync()
    {
        if (!_completed)
        {
            await RollbackAsync();
        }
    }
}
