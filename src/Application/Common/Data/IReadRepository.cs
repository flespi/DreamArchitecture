using System.Linq.Expressions;
using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Application.Common.Data;

/// <summary>
/// <para>
/// A <see cref="IRepository{T}" /> can be used to query instances of <typeparamref name="T" />.
/// An <see cref="QueryContext{T}"/> (or derived) is used to encapsulate the LINQ queries against the database.
/// </para>
/// </summary>
/// <typeparam name="T">The type of entity being operated on by this repository.</typeparam>
public interface IReadRepository<T> where T : class, IAggregateRoot
{
    /// <summary>
    /// Finds an entity with the given primary key value.
    /// </summary>
    /// <typeparam name="TId">The type of primary key.</typeparam>
    /// <param name="id">The value of the primary key for the entity to be found.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the <typeparamref name="T" />, or <see langword="null"/>.
    /// </returns>
    Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull;

    /// <summary>
    /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the <typeparamref name="T" />, or <see langword="null"/>.
    /// </returns>
    Task<T?> FirstOrDefaultAsync(QueryContext<T>? specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the <typeparamref name="T" />, or <see langword="null"/>.
    /// </returns>
    Task<T?> SingleOrDefaultAsync(QueryContext<T>? specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds all entities of <typeparamref name="T" /> from the database.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
    /// </returns>
    Task<List<T>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds all entities of <typeparamref name="T" />, that matches the encapsulated query logic of the
    /// <paramref name="specification"/>, from the database.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
    /// </returns>
    Task<List<T>> ListAsync(QueryContext<T>? specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a number that represents how many entities satisfy the encapsulated query logic
    /// of the <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">The encapsulated query logic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the
    /// number of elements in the input sequence.
    /// </returns>
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the total number of records.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the
    /// number of elements in the input sequence.
    /// </returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a boolean that represents whether any entity satisfy the encapsulated query logic
    /// of the <paramref name="predicate"/> or not.
    /// </summary>
    /// <param name="predicate">The encapsulated query logic.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains true if the 
    /// source sequence contains any elements; otherwise, false.
    /// </returns>
    Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a boolean whether any entity exists or not.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains true if the 
    /// source sequence contains any elements; otherwise, false.
    /// </returns>
    Task<bool> AnyAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds all entities of <typeparamref name="T" />, that matches the encapsulated query logic of the
    /// <paramref name="specification"/>, from the database.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <returns>
    ///  Returns an IAsyncEnumerable which can be enumerated asynchronously.
    /// </returns>
    IAsyncEnumerable<T> AsAsyncEnumerable(QueryContext<T>? specification);

    /// <summary>
    /// Retrieves a paginated subset of entities of <typeparamref name="T" /> from the database.
    /// </summary>
    /// <param name="arguments">The paging arguments that define page size, page number, and sorting.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="Page{T}" /> with the requested page of entities
    /// and associated pagination metadata.
    /// </returns>
    Task<Page<T>> GetPageAsync(PagingArguments arguments, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated subset of entities of <typeparamref name="T" /> that satisfy the
    /// encapsulated query logic of the <paramref name="specification"/>.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="arguments">The paging arguments that define page size, page number, and sorting.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="Page{T}" /> with the requested page of entities
    /// and associated pagination metadata.
    /// </returns>
    Task<Page<T>> GetPageAsync(QueryContext<T>? specification, PagingArguments arguments, CancellationToken cancellationToken = default);
}
