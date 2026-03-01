using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Application.Common.Data;

/// <summary>
/// Represents a Unit of Work that coordinates the work of multiple repositories
/// and manages transactional boundaries.
/// </summary>
public interface IUnitOfWorkBase : ITransactional
{
    /// <summary>
    /// Gets a repository instance for the specified aggregate root type.
    /// </summary>
    /// <typeparam name="T">The type of the aggregate root.</typeparam>
    /// <returns>
    /// An <see cref="IRepository{T}"/> instance that can be used to query and
    /// persist aggregates of type <typeparamref name="T"/>.
    /// </returns>
    IRepository<T> Repository<T>() where T : class, IAggregateRoot;

    /// <summary>
    /// Persists all changes made in the current unit of work to the underlying data store.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the number of state entries written to the data store.
    /// </returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
