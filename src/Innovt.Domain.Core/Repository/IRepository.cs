// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Collections;
using Innovt.Domain.Core.Specification;

namespace Innovt.Domain.Core.Repository;

/// <summary>
///     Represents a generic repository for data access operations.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    ///     Adds a single entity
    /// </summary>
    /// <param name="entity"></param>
    void Add(T entity);

    /// <summary>
    ///     Adds multiple entities
    /// </summary>
    /// <param name="entities"></param>
    void Add(IEnumerable<T> entities);

    /// <summary>
    ///     Asynchronously adds a single entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task AddAsync(T entity);

    /// <summary>
    ///     Asynchronously adds multiple entities
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    Task AddAsync(IEnumerable<T> entities);

    /// <summary>
    ///     Modifies an entity
    /// </summary>
    /// <param name="entity"></param>
    void Modify(T entity);

    /// <summary>
    ///     Removes multiple entities
    /// </summary>
    /// <param name="entities"></param>
    void Remove(IEnumerable<T> entities);

    /// <summary>
    ///     Removes a single entity
    /// </summary>
    /// <param name="entity"></param>
    void Remove(T entity);

    /// <summary>
    ///     Gets a single entity based on a specification and includes related entities
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    T? GetSingleOrDefault(ISpecification<T> specification, Include? includes = null);

    /// <summary>
    ///     Asynchronously gets a single entity based on a specification and includes related entities
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="includes"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<T?> GetSingleOrDefaultAsync(ISpecification<T> specification, Include? includes = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets the first entity based on a specification and includes related entities
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    T? GetFirstOrDefault(ISpecification<T> specification, Include? includes = null);

    /// <summary>
    ///     Asynchronously gets the first entity based on a specification and includes related entities
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="includes"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<T?> GetFirstOrDefaultAsync(ISpecification<T> specification, Include? includes = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds entities based on a specification and includes related entities
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    IEnumerable<T> FindBy(ISpecification<T> specification, Include? includes = null);

    /// <summary>
    ///     Finds entities based on a specification, with optional ordering, and includes related entities
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="specification"></param>
    /// <param name="orderBy"></param>
    /// <param name="isOrderByDescending"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    IEnumerable<T> FindBy<TKey>(ISpecification<T> specification, Expression<Func<T, TKey>>? orderBy = null,
        bool isOrderByDescending = false, Include? includes = null);

    /// <summary>
    ///     Asynchronously finds entities based on a specification and includes related entities
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="includes"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<T>> FindByAsync(ISpecification<T> specification, Include? includes = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously finds entities based on a specification, with optional ordering, and includes related entities
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="specification"></param>
    /// <param name="orderBy"></param>
    /// <param name="isOrderByDescending"></param>
    /// <param name="includes"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<T>> FindByAsync<TKey>(ISpecification<T> specification,
        Expression<Func<T, TKey>>? orderBy = null, bool isOrderByDescending = false, Include? includes = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds entities based on a specification and returns a paged collection, with optional includes
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    PagedCollection<T> FindPaginatedBy(ISpecification<T> specification, Include? includes = null);

    /// <summary>
    ///     Finds entities based on a specification, with optional ordering, and returns a paged collection, with optional
    ///     includes
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="specification"></param>
    /// <param name="orderBy"></param>
    /// <param name="isOrderByDescending"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    PagedCollection<T> FindPaginatedBy<TKey>(ISpecification<T> specification,
        Expression<Func<T, TKey>>? orderBy = null,
        bool isOrderByDescending = false, Include? includes = null);

    /// <summary>
    ///     Asynchronously finds entities based on a specification and returns a paged collection, with optional includes
    /// </summary>
    /// <param name="specification"></param>
    /// <param name="includes"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PagedCollection<T>> FindPaginatedByAsync(ISpecification<T> specification, Include? includes = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously finds entities based on a specification, with optional ordering, and returns a paged collection,
    ///     with optional includes
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="specification"></param>
    /// <param name="orderBy"></param>
    /// <param name="isOrderByDescending"></param>
    /// <param name="includes"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PagedCollection<T>> FindPaginatedByAsync<TKey>(ISpecification<T> specification,
        Expression<Func<T, TKey>>? orderBy = null,
        bool isOrderByDescending = false, Include? includes = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <typeparam name="TKEntity"></typeparam>
    /// <param name="specification"></param>
    /// <returns></returns>
    int CountBy<TKEntity>(ISpecification<TKEntity> specification) where TKEntity : class;

    /// <summary>
    ///     Counts entities based on a specificatio
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    int CountBy(ISpecification<T> specification);

    /// <summary>
    ///     Asynchronously counts entities based on a specification for a given entity type
    /// </summary>
    /// <typeparam name="TKEntity"></typeparam>
    /// <param name="specification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> CountByAsync<TKEntity>(ISpecification<TKEntity> specification, CancellationToken cancellationToken = default) where TKEntity : class;

    Task<int> CountByAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
}