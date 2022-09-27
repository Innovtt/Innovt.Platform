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

public class RepositoryBase<T> : IRepository<T> where T : class
{
    protected IExtendedUnitOfWork Context;

    public RepositoryBase(IExtendedUnitOfWork context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public virtual void Add(T entity)
    {
        Context.Add(entity);
    }

    public virtual void Add(IEnumerable<T> entities)
    {
        Context.Add(entities);
    }

    public virtual Task AddAsync(IEnumerable<T> entities)
    {
        return Context.AddAsync(entities);
    }

    public virtual Task AddAsync(T entity)
    {
        return Context.AddAsync(entity);
    }

    public virtual void Modify(T entity)
    {
        Context.Update(entity);
    }

    public virtual void Remove(IEnumerable<T> entities)
    {
        Context.Remove(entities);
    }

    public virtual void Remove(T entity)
    {
        Context.Remove(entity);
    }

    public virtual T GetFirstOrDefault(ISpecification<T> specification, Include includes = null)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        return Context.Queryable<T>().AddInclude(includes).FirstOrDefault(specification.SatisfiedBy());
    }

    public virtual async Task<T> GetFirstOrDefaultAsync(ISpecification<T> specification, Include includes = null,
        CancellationToken cancellationToken = default)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        return await Context.Queryable<T>().AddInclude(includes)
            .FirstOrDefaultAsync(specification.SatisfiedBy(), cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task<T> GetSingleOrDefaultAsync(ISpecification<T> specification, Include includes = null,
        CancellationToken cancellationToken = default)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        return await Context.Queryable<T>().AddInclude(includes)
            .SingleOrDefaultAsync(specification.SatisfiedBy(), cancellationToken).ConfigureAwait(false);
    }

    public virtual T GetSingleOrDefault(ISpecification<T> specification, Include includes = null)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        return Context.Queryable<T>().AddInclude(includes).SingleOrDefault(specification.SatisfiedBy());
    }

    public virtual IEnumerable<T> FindBy(ISpecification<T> specification, Include includes = null)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        var query = Context.Queryable<T>().AddInclude(includes)
            .Where(specification.SatisfiedBy())
            .ApplyPagination(specification);

        return query.ToList();
    }


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


    public virtual int CountBy(ISpecification<T> specification)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        return Context.Queryable<T>().Count(specification.SatisfiedBy());
    }

    public virtual int CountBy<TKEntity>(ISpecification<TKEntity> specification) where TKEntity : class
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        return Context.Queryable<TKEntity>().Count(specification.SatisfiedBy());
    }

    public virtual async Task<int> CountByAsync<TKEntity>(ISpecification<TKEntity> specification,
        CancellationToken cancellationToken = default) where TKEntity : class
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        return await Context.Queryable<TKEntity>().CountAsync(specification.SatisfiedBy(), cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<int> CountByAsync(ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        if (specification == null) throw new ArgumentNullException(nameof(specification));

        return await Context.Queryable<T>().CountAsync(specification.SatisfiedBy(), cancellationToken)
            .ConfigureAwait(false);
    }
}