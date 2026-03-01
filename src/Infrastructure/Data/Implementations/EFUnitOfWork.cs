using CleanArchitecture.Application.Common.Data;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Infrastructure.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data.Implementations;

public class EFUnitOfWork : IUnitOfWork
{
    public DbContext DbContext { get; }

    public IAggregateBoundaryProvider BoundaryProvider { get; }

    public EFUnitOfWork(DbContext dbContext, IAggregateBoundaryProvider boundaryProvider)
    {
        DbContext = dbContext;
        BoundaryProvider = boundaryProvider;
    }

    public IRepository<T> Repository<T>() where T : class, IAggregateRoot
    {
        return new EFRepository<T>(DbContext, BoundaryProvider);
    }

    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await DbContext.Database.BeginTransactionAsync(cancellationToken);
        return new EFTransaction(transaction);
    }
}
