using CleanArchitecture.Application;
using CleanArchitecture.Application.Common.Identity;
using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitecture.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly IIdentityAccessor _identityAccessor;
    private readonly TimeProvider _dateTime;

    public AuditableEntityInterceptor(
        IIdentityAccessor identityAccessor,
        TimeProvider dateTime)
    {
        _identityAccessor = identityAccessor;
        _dateTime = dateTime;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return ValueTask.FromResult(result);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                var utcNow = _dateTime.GetUtcNow();

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.Audit = new();
                    entry.Entity.Audit.CreatedBy = _identityAccessor.Principal?.GetSubject();
                    entry.Entity.Audit.Created = utcNow;
                }

                entry.Entity.Audit.LastModifiedBy = _identityAccessor.Principal?.GetSubject();
                entry.Entity.Audit.LastModified = utcNow;
            }
        }
    }
}
