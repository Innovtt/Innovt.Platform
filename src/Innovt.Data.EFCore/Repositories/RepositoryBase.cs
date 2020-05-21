using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Collections;
using Innovt.Domain.Repository;
using Innovt.Domain.Specification;
using Microsoft.EntityFrameworkCore;

namespace Innovt.Data.EFCore.Repositories
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected IExtendedUnitOfWork Context;

        public RepositoryBase(IExtendedUnitOfWork context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual void Add(T entity) => Context.Add(entity);

        public virtual Task AddAsync(T entity) => Context.AddAsync(entity);
        
        public virtual void Modify(T entity) => Context.Update(entity);
        
        public virtual void Remove(IEnumerable<T> entities) => Context.Remove(entities);
        public virtual void Remove(T entity) => Context.Remove(entity);
      
        public virtual T GetFirstOrDefault(ISpecification<T> specification, Include includes = null)
           => Context.Queryable<T>().AddInclude(includes).FirstOrDefault(specification.SatisfiedBy()); 

        public virtual async Task<T> GetFirstOrDefaultAsync(ISpecification<T> specification, Include includes = null,CancellationToken cancellationToken = default)
        => await Context.Queryable<T>().AddInclude(includes).FirstOrDefaultAsync(specification.SatisfiedBy(),cancellationToken); 
       
        public virtual async Task<T> GetSingleOrDefaultAsync(ISpecification<T> specification, Include includes=null,CancellationToken cancellationToken = default)
          => await Context.Queryable<T>().AddInclude(includes).SingleOrDefaultAsync(specification.SatisfiedBy(),cancellationToken); 

        public virtual T GetSingleOrDefault(ISpecification<T> specification, Include includes = null)
            => Context.Queryable<T>().AddInclude(includes).SingleOrDefault(specification.SatisfiedBy()); 
     
        public virtual IEnumerable<T> FindBy(ISpecification<T> specification, Include includes = null)
        {
            var query = Context.Queryable<T>().AddInclude(includes)
                .Where(specification.SatisfiedBy())
                .ApplyPagination(specification);

            return query.ToList();
        }

        
        public virtual IEnumerable<T> FindBy<TKey>(ISpecification<T> specification,
            System.Linq.Expressions.Expression<Func<T, TKey>> orderBy = null, bool isOrderByDescending = false, Include includes = null)
        {
            var query = Context.Queryable<T>()
                .AddInclude(includes)
                .Where(specification.SatisfiedBy())
                .ApplyPagination(specification);

            if (orderBy != null)
            {
                query = isOrderByDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            }

            return query;
        }

        public virtual async Task<IEnumerable<T>> FindByAsync(ISpecification<T> specification,
            Include includes = null,
            CancellationToken cancellationToken = default)
        {
            var query = Context.Queryable<T>().AddInclude(includes)
                .Where(specification.SatisfiedBy())
                .ApplyPagination(specification);
            
            return await query.ToListAsync(cancellationToken);
        }
        
        public virtual async Task<IEnumerable<T>> FindByAsync<TKey>(ISpecification<T> specification,
            System.Linq.Expressions.Expression<Func<T, TKey>> orderBy = null,
            bool isOrderByDescending = false, Include includes = null, 
            CancellationToken cancellationToken = default)
        {
            var query = Context.Queryable<T>().AddInclude(includes)
                .Where(specification.SatisfiedBy())
                .ApplyPagination(specification);

            if (orderBy != null)
            {
                query = isOrderByDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            }
            
            return await query.ToListAsync(cancellationToken);
        }

        public virtual PagedCollection<T> FindPaginatedBy(ISpecification<T> specification, Include includes = null)
        {
            var items  =  this.FindBy(specification, includes);

            var result = new PagedCollection<T>(items,specification.Page,specification.PageSize)
            {
                TotalRecords =  this.CountBy(specification)
            };
           
            return result;
        }

        public virtual PagedCollection<T> FindPaginatedBy<TKey>(ISpecification<T> specification,
            System.Linq.Expressions.Expression<Func<T, TKey>> orderBy = null,
            bool isOrderByDescending = false, Include includes = null)
        {

            var items = this.FindBy<TKey>(specification,orderBy,isOrderByDescending, includes);
            
            var result = new PagedCollection<T>(items,specification.Page,specification.PageSize)
            {
                TotalRecords =  this.CountBy(specification)
            };
           
            return result;
        }
        
        public virtual async Task<PagedCollection<T>> FindPaginatedByAsync(ISpecification<T> specification, Include includes = null, CancellationToken cancellationToken = default)
        {
          var items = await this.FindByAsync(specification, includes,cancellationToken);

            var result = new PagedCollection<T>(items,specification.Page,specification.PageSize)
            {
                TotalRecords = await this.CountByAsync(specification, cancellationToken)
            };
           
            return result;
        }

        public virtual async Task<PagedCollection<T>> FindPaginatedByAsync<TKey>(ISpecification<T> specification, System.Linq.Expressions.Expression<Func<T, TKey>> orderBy = null, bool isOrderByDescending = false, Include includes = null, CancellationToken cancellationToken = default)
        {
            var   items        = await this.FindByAsync<TKey>(specification,orderBy,isOrderByDescending, includes,cancellationToken);
            
            var result = new PagedCollection<T> (items,specification.Page,specification.PageSize)
            {
                TotalRecords = await this.CountByAsync(specification, cancellationToken)
            };
           
            return result;
        }

     
        public virtual int CountBy(ISpecification<T> specification)
            => Context.Queryable<T>().Count(specification.SatisfiedBy());

        public virtual int CountBy<TKEntity>(ISpecification<TKEntity> specification) where TKEntity : class
            =>  Context.Queryable<TKEntity>().Count(specification.SatisfiedBy());

        public virtual async Task<int> CountByAsync<TKEntity>(ISpecification<TKEntity> specification, CancellationToken cancellationToken = default) where TKEntity : class
            =>  await Context.Queryable<TKEntity>().CountAsync(specification.SatisfiedBy(),cancellationToken);

        public async Task<int> CountByAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
            => await Context.Queryable<T>().CountAsync(specification.SatisfiedBy(), cancellationToken);
    }
}