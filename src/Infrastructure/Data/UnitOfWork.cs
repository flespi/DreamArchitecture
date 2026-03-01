using CleanArchitecture.Infrastructure.Data.Abstractions;
using CleanArchitecture.Infrastructure.Data.Implementations;

namespace CleanArchitecture.Infrastructure.Data;

public class UnitOfWork : EFUnitOfWork
{
    public UnitOfWork(ApplicationDbContext dbContext, IAggregateBoundaryProvider boundaryProvider)
        : base(dbContext, boundaryProvider)
    {
    }
}
