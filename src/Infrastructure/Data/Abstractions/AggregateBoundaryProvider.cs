using CleanArchitecture.Domain.Common;
using EntityStitching;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Infrastructure.Data.Abstractions;

public class AggregateBoundaryProvider : IAggregateBoundaryProvider
{
    private readonly IServiceProvider _serviceProvider;

    public AggregateBoundaryProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IAggregateBoundary<TEntity> GetBoundary<TEntity>()
        where TEntity : IAggregateRoot
        => _serviceProvider.GetService<IAggregateBoundary<TEntity>>()
            ?? new AggregateBoundary<TEntity>();
}
