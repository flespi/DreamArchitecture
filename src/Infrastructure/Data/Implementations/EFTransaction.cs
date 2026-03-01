using CleanArchitecture.Application.Common.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace CleanArchitecture.Infrastructure.Data.Implementations;

public class EFTransaction : Transaction
{
    private readonly IDbContextTransaction _transaction;

    public EFTransaction(IDbContextTransaction transaction)
    {
        _transaction = transaction;
    }

    protected override async Task ExecuteCommitAsync(CancellationToken cancellationToken = default)
    {
        await _transaction.CommitAsync(cancellationToken);
    }

    protected override async Task ExecuteRollbackAsync(CancellationToken cancellationToken = default)
    {
        await _transaction.RollbackAsync(cancellationToken);
    }
}
