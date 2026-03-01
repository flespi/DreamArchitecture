using System.Linq.Expressions;
using CleanArchitecture.Application.Common.Data;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Infrastructure.Data.Abstractions;
using EntityStitching;
using GreenDonut.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data.Implementations;

public class EFRepository<T> : IRepository<T> where T : class, IAggregateRoot
{
    protected DbContext DbContext { get; }

    protected DbSet<T> DbSet => DbContext.Set<T>();

    protected IAggregateBoundary<T> Boundary { get; }

    public EFRepository(DbContext dbContext, IAggregateBoundaryProvider boundaryProvider)
    {
        DbContext = dbContext;
        Boundary = boundaryProvider.GetBoundary<T>();
    }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await DbSet.AddRangeAsync(entities, cancellationToken);
    }

    public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Update(entity);

        return Task.CompletedTask;
    }

    public virtual Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        DbContext.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(entity);
        
        return Task.CompletedTask;
    }

    public virtual Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        DbSet.RemoveRange(entities);

        return Task.CompletedTask;
    }

    public virtual Task DeleteRangeAsync(QueryContext<T>? specification, CancellationToken cancellationToken = default)
    {
        var query = DbSet.With(specification);
        DbSet.RemoveRange(query);

        return Task.CompletedTask;
    }

    public virtual async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
    {
        var predicate = DbContext.CreateGetByKeyPredicate<T>(id);

        return await DbSet.Include(Boundary).FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public virtual async Task<T?> FirstOrDefaultAsync(QueryContext<T>? specification, CancellationToken cancellationToken = default)
    {
        var source = DbSet.AsQueryable();

        source = specification?.Selector is null
            ? source.Include(Boundary)
            : source.AsNoTracking();

        return await source.With(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<T?> SingleOrDefaultAsync(QueryContext<T>? specification, CancellationToken cancellationToken = default)
    {
        var source = DbSet.AsQueryable();

        source = specification?.Selector is null
            ? source.Include(Boundary)
            : source.AsNoTracking();

        return await source.With(specification).SingleOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.Include(Boundary).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<T>> ListAsync(QueryContext<T>? specification, CancellationToken cancellationToken = default)
    {
        var source = DbSet.AsQueryable();

        source = specification?.Selector is null
            ? source.Include(Boundary)
            : source.AsNoTracking();

        return await source.With(specification).ToListAsync(cancellationToken);
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate, CancellationToken cancellationToken = default)
    {
        var source = DbSet.AsQueryable();

        if (predicate is not null)
        {
            source = source.Where(predicate); 
        }

        return await source.CountAsync(cancellationToken);
    }

    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.CountAsync(cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate, CancellationToken cancellationToken = default)
    {
        var source = DbSet.AsQueryable();

        if (predicate is not null)
        {
            source = source.Where(predicate);
        }

        return await source.AnyAsync(cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(cancellationToken);
    }

    public virtual IAsyncEnumerable<T> AsAsyncEnumerable(QueryContext<T>? specification)
    {
        var source = DbSet.AsQueryable();

        source = specification?.Selector is null
            ? source.Include(Boundary)
            : source.AsNoTracking();

        return source.With(specification).AsAsyncEnumerable();
    }

    public virtual async Task<Page<T>> GetPageAsync(PagingArguments arguments, CancellationToken cancellationToken = default)
    {
        return await DbSet.Include(Boundary).ToPageAsync(arguments, cancellationToken);
    }

    public virtual async Task<Page<T>> GetPageAsync(QueryContext<T>? specification, PagingArguments arguments, CancellationToken cancellationToken = default)
    {
        var source = DbSet.AsQueryable();

        source = specification?.Selector is null
            ? source.Include(Boundary)
            : source.AsNoTracking();

        return await source.With(specification).ToPageAsync(arguments, cancellationToken);
    }
}
