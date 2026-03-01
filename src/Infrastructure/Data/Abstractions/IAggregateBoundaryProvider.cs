using CleanArchitecture.Domain.Common;
using EntityStitching;

namespace CleanArchitecture.Infrastructure.Data.Abstractions;

public interface IAggregateBoundaryProvider
{
    IAggregateBoundary<TEntity> GetBoundary<TEntity>()
        where TEntity : IAggregateRoot;
}
