// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.EFCore

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Collections;
using Innovt.Domain.Core.Repository;
using Innovt.Domain.Core.Specification;
using Microsoft.EntityFrameworkCore;

namespace Innovt.Data.EFCore.Repositories;
/// <summary>
/// Base repository providing common functionality for accessing and managing entities of type T.
/// Implements the IRepository interface.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public class RepositoryBase<T> : IRepository<T> where T : class
{
    /// <summary>
    /// The extended unit of work context for interacting with the database.
    /// </summary>
    protected IExtendedUnitOfWork Context;

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class.
    /// </summary>
    /// <param name="context">The extended unit of work context.</param>
    /// <exception cref="ArgumentNullException">Thrown when the context parameter is null.</exception>
    public RepositoryBase(IExtendedUnitOfWork context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }
    /// <inheritdoc/>
    public virtual void Add(T entity)
    {
        Context.Add(entity);
    }
    /// <inheritdoc/>
    public virtual void Add(IEnumerable<T> entities)
    {
        Context.Add(entities);
    }
    /// <summary>
    /// Asynchronously adds a collection of entities of type T to the repository.
    /// </summary>
    /// <param name="entities">The collection of entities to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual Task AddAsync(IEnumerable<T> entities)
    {
        return Context.AddAsync(entities);
    }
    /// <summary>
    /// Asynchronously adds an entity of type T to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual Task AddAsync(T entity)
    {
        return Context.AddAsync(entity);
    }
    /// <summary>
    /// Modifies an entity of type T in the repository.
    /// </summary>
    /// <param name="entity">The entity to modify.</param>
    public virtual void Modify(T entity)
    {
        Context.Update(entity);
    }
    /// <summary>
    /// Removes a collection of entities of type T from the repository.
    /// </summary>
    /// <param name="entities">The collection of entities to remove.</param>
    public virtual void Remove(IEnumerable<T> entities)
    {
        Context.Remove(entities);
    }
    /// <summary>
    /// Removes an entity of type T from the repository.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    public virtual void Remove(T entity)
    {
        Context.Remove(entity);
    }
    /// <summary>
    /// Gets the first or default entity of type T based on the provided specification.
    /// </summary>
    /// <param name="specification">The specification used to filter the entity.</param>
    /// <param name="includes">The related entities to include in the query.</param>
    /// <returns>The first or default entity matching the specification.</returns>
    public virtual T GetFirstOrDefault(ISpecification<T> specification, Include includes = null)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        return Context.Queryable<T>().AddInclude(includes).FirstOrDefault(specification.SatisfiedBy());
    }
    /// <summary>
    /// Asynchronously gets the first or default entity of type T based on the provided specification.
    /// </summary>
    /// <param name="specification">The specification used to filter the entity.</param>
    /// <param name="includes">The related entities to include in the query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and the first or default entity matching the specification.</returns>
    public virtual async Task<T> GetFirstOrDefaultAsync(ISpecification<T> specification, Include includes = null,
        CancellationToken cancellationToken = default)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        return await Context.Queryable<T>().AddInclude(includes)
            .FirstOrDefaultAsync(specification.SatisfiedBy(), cancellationToken).ConfigureAwait(false);
    }
    /// <summary>
    /// Asynchronously gets the single or default entity of type T based on the provided specification.
    /// </summary>
    /// <param name="specification">The specification used to filter the entity.</param>
    /// <param name="includes">The related entities to include in the query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and the single or default entity matching the specification.</returns>
    public virtual async Task<T> GetSingleOrDefaultAsync(ISpecification<T> specification, Include includes = null,
        CancellationToken cancellationToken = default)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        return await Context.Queryable<T>().AddInclude(includes)
            .SingleOrDefaultAsync(specification.SatisfiedBy(), cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the single or default entity of type T based on the provided specification.
    /// </summary>
    /// <param name="specification">The specification used to filter the entity.</param>
    /// <param name="includes">The related entities to include in the query.</param>
    /// <returns>The single or default entity matching the specification.</returns>
    public virtual T GetSingleOrDefault(ISpecification<T> specification, Include includes = null)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        return Context.Queryable<T>().AddInclude(includes).SingleOrDefault(specification.SatisfiedBy());
    }
    /// <summary>
    /// Finds entities of type T based on the provided specification with optional sorting and pagination.
    /// </summary>
    /// <typeparam name="TKey">The type of the sorting key.</typeparam>
    /// <param name="specification">The specification used to filter the entities.</param>
    /// <param name="orderBy">The sorting key selector.</param>
    /// <param name="isOrderByDescending">A flag indicating whether the sorting is in descending order.</param>
    /// <param name="includes">The related entities to include in the query.</param>
    /// <returns>An enumerable of entities matching the specification with optional sorting.</returns>
    public virtual IEnumerable<T> FindBy(ISpecification<T> specification, Include includes = null)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        var query = Context.Queryable<T>().AddInclude(includes)
            .Where(specification.SatisfiedBy())
            .ApplyPagination(specification);

        return query.ToList();
    }

    /// <summary>
    /// Finds entities of type T based on the provided specification with optional sorting and pagination.
    /// </summary>
    /// <typeparam name="TKey">The type of the sorting key.</typeparam>
    /// <param name="specification">The specification used to filter the entities.</param>
    /// <param name="orderBy">The sorting key selector.</param>
    /// <param name="isOrderByDescending">A flag indicating whether the sorting is in descending order.</param>
    /// <param name="includes">The related entities to include in the query.</param>
    /// <returns>An enumerable of entities matching the specification with optional sorting.</returns>
    public virtual IEnumerable<T> FindBy<TKey>(ISpecification<T> specification,
        Expression<Func<T, TKey>>? orderBy = null, bool isOrderByDescending = false,
        Include includes = null)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        var query = Context.Queryable<T>()
            .AddInclude(includes)
            .Where(specification.SatisfiedBy());


        if (orderBy != null)
            query = isOrderByDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);

        query = query.ApplyPagination(specification);

        return query;
    }

    /// <summary>
    /// Asynchronously finds entities of type T based on the provided specification with optional sorting and pagination.
    /// </summary>
    /// <param name="specification">The specification used to filter the entities.</param>
    /// <param name="includes">The related entities to include in the query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and an enumerable of entities matching the specification.</returns>
    public virtual async Task<IEnumerable<T>> FindByAsync(ISpecification<T> specification,
        Include includes = null,
        CancellationToken cancellationToken = default)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        var query = Context.Queryable<T>().AddInclude(includes)
            .Where(specification.SatisfiedBy())
            .ApplyPagination(specification);

        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }
    /// <summary>
    /// Asynchronously finds entities of type T based on the provided specification with optional sorting and pagination.
    /// </summary>
    /// <typeparam name="TKey">The type of the sorting key.</typeparam>
    /// <param name="specification">The specification used to filter the entities.</param>
    /// <param name="orderBy">The sorting key selector.</param>
    /// <param name="isOrderByDescending">A flag indicating whether the sorting is in descending order.</param>
    /// <param name="includes">The related entities to include in the query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation and an enumerable of entities matching the specification with optional sorting.</returns>
    public virtual async Task<IEnumerable<T>> FindByAsync<TKey>(ISpecification<T> specification,
        Expression<Func<T, TKey>>? orderBy = null,
        bool isOrderByDescending = false, Include includes = null,
        CancellationToken cancellationToken = default)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        var query = Context.Queryable<T>().AddInclude(includes)
            .Where(specification.SatisfiedBy());

        if (orderBy != null)
            query = isOrderByDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);


        query = query.ApplyPagination(specification);


        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }
    /// <summary>
    /// Finds paginated entities of type T based on the provided specification.
    /// </summary>
    /// <param name="specification">The specification used to filter the entities.</param>
    /// <param name="includes">The related entities to include in the query.</param>
    /// <returns>A paged collection of entities matching the specification.</returns>
    public virtual PagedCollection<T> FindPaginatedBy(ISpecification<T> specification, Include includes = null)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        var items = FindBy(specification, includes);

        var result = new PagedCollection<T>(items, specification.Page, specification.PageSize)
        {
            TotalRecords = CountBy(specification)
        };

        return result;
    }
    /// <summary>
    /// Retrieves a paginated collection of entities based on the provided specification, with optional sorting and included navigation properties.
    /// </summary>
    /// <typeparam name="TKey">The type of the key used for sorting.</typeparam>
    /// <param name="specification">The specification to filter the entities.</param>
    /// <param name="orderBy">Expression to order the results (optional).</param>
    /// <param name="isOrderByDescending">Flag to determine descending order for sorting (default: ascending).</param>
    /// <param name="includes">Navigation properties to include (optional).</param>
    /// <returns>A paged collection of entities satisfying the specified criteria.</returns>
    public virtual PagedCollection<T> FindPaginatedBy<TKey>(ISpecification<T> specification,
        Expression<Func<T, TKey>>? orderBy = null,
        bool isOrderByDescending = false, Include includes = null)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        var items = FindBy(specification, orderBy, isOrderByDescending, includes);

        var result = new PagedCollection<T>(items, specification.Page, specification.PageSize)
        {
            TotalRecords = CountBy(specification)
        };

        return result;
    }
    /// <summary>
    /// Asynchronously retrieves a paginated collection of entities based on the provided specification, with optional included navigation properties.
    /// </summary>
    /// <param name="specification">The specification to filter the entities.</param>
    /// <param name="includes">Navigation properties to include (optional).</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>A task that represents the asynchronous operation, yielding a paged collection of entities satisfying the specified criteria.</returns>
    public virtual async Task<PagedCollection<T>> FindPaginatedByAsync(ISpecification<T> specification,
        Include includes = null, CancellationToken cancellationToken = default)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        var items = await FindByAsync(specification, includes, cancellationToken).ConfigureAwait(false);

        var result = new PagedCollection<T>(items, specification.Page, specification.PageSize)
        {
            TotalRecords = await CountByAsync(specification, cancellationToken).ConfigureAwait(false)
        };

        return result;
    }
    /// <summary>
    /// Asynchronously retrieves a paginated collection of entities based on the provided specification, with optional sorting and included navigation properties.
    /// </summary>
    /// <typeparam name="TKey">The type of the key used for sorting.</typeparam>
    /// <param name="specification">The specification to filter the entities.</param>
    /// <param name="orderBy">Expression to order the results (optional).</param>
    /// <param name="isOrderByDescending">Flag to determine descending order for sorting (default: ascending).</param>
    /// <param name="includes">Navigation properties to include (optional).</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>A task that represents the asynchronous operation, yielding a paged collection of entities satisfying the specified criteria.</returns>
    public virtual async Task<PagedCollection<T>> FindPaginatedByAsync<TKey>(ISpecification<T> specification,
        Expression<Func<T, TKey>>? orderBy = null, bool isOrderByDescending = false,
        Include includes = null, CancellationToken cancellationToken = default)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        var items = await FindByAsync(specification, orderBy, isOrderByDescending, includes,
            cancellationToken).ConfigureAwait(false);

        var result = new PagedCollection<T>(items, specification.Page, specification.PageSize)
        {
            TotalRecords = await CountByAsync(specification, cancellationToken).ConfigureAwait(false)
        };

        return result;
    }
    /// <summary>
    /// Counts the number of entities that satisfy the specified specification.
    /// </summary>
    /// <param name="specification">The specification to filter the entities.</param>
    /// <returns>The total count of entities satisfying the specified criteria.</returns>
    public virtual int CountBy(ISpecification<T> specification)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        return Context.Queryable<T>().Count(specification.SatisfiedBy());
    }
    /// <summary>
    /// Counts the number of entities of a specified type that satisfy the specified specification.
    /// </summary>
    /// <typeparam name="TKEntity">The type of the entity to count.</typeparam>
    /// <param name="specification">The specification to filter the entities.</param>
    /// <returns>The total count of entities satisfying the specified criteria.</returns>
    public virtual int CountBy<TKEntity>(ISpecification<TKEntity> specification) where TKEntity : class
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        return Context.Queryable<TKEntity>().Count(specification.SatisfiedBy());
    }
    /// <summary>
    /// Asynchronously counts the number of entities of a specified type that satisfy the specified specification.
    /// </summary>
    /// <typeparam name="TKEntity">The type of the entity to count.</typeparam>
    /// <param name="specification">The specification to filter the entities.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>A task that represents the asynchronous operation, yielding the total count of entities satisfying the specified criteria.</returns>
    public virtual async Task<int> CountByAsync<TKEntity>(ISpecification<TKEntity> specification,
        CancellationToken cancellationToken = default) where TKEntity : class
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        return await Context.Queryable<TKEntity>().CountAsync(specification.SatisfiedBy(), cancellationToken)
            .ConfigureAwait(false);
    }
    /// <summary>
    /// Asynchronously counts the number of entities that satisfy the specified specification.
    /// </summary>
    /// <param name="specification">The specification to filter the entities.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>A task that represents the asynchronous operation, yielding the total count of entities satisfying the specified criteria.</returns>
    public async Task<int> CountByAsync(ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        return await Context.Queryable<T>().CountAsync(specification.SatisfiedBy(), cancellationToken)
            .ConfigureAwait(false);
    }
}