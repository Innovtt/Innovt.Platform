using Innovt.Core.Collections;
using Innovt.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Utilities;

namespace Innovt.Domain.Repository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        Task AddAsync(T entity);

        void Remove(T entity);

        void Modify(T entity);

        void Remove(IEnumerable<T> entities);

        T GetSingleOrDefault(ISpecification<T> specification, Include includes=null);
    
        Task<T> GetSingleOrDefaultAsync(ISpecification<T> specification, Include includes=null,  
            CancellationToken cancellationToken = default);
        
        T GetFirstOrDefault(ISpecification<T> specification,  Include includes=null);

        Task<T> GetFirstOrDefaultAsync(ISpecification<T> specification,Include includes=null,CancellationToken cancellationToken = default);
        
        IEnumerable<T> FindBy(ISpecification<T> specification, Include includes = null);

        IEnumerable<T> FindBy<TKey>(ISpecification<T> specification, Expression<Func<T, TKey>> orderBy = null,
            bool isOrderByDescending = false, Include includes = null);
        
        Task<IEnumerable<T>> FindByAsync(ISpecification<T> specification,Include includes = null,CancellationToken cancellationToken = default);
        
        Task<IEnumerable<T>> FindByAsync<TKey>(ISpecification<T> specification,
            Expression<Func<T, TKey>> orderBy = null, bool isOrderByDescending = false, Include includes = null,CancellationToken cancellationToken = default);
 
        PagedCollection<T> FindPaginatedBy(ISpecification<T> specification, Include includes = null);

        PagedCollection<T> FindPaginatedBy<TKey>(ISpecification<T> specification, Expression<Func<T, TKey>> orderBy = null, 
            bool isOrderByDescending = false,Include includes = null);

        Task<PagedCollection<T>> FindPaginatedByAsync(ISpecification<T> specification, Include includes = null,CancellationToken cancellationToken = default);
        
        Task<PagedCollection<T>> FindPaginatedByAsync<TKey>(ISpecification<T> specification, Expression<Func<T, TKey>> orderBy = null, 
            bool isOrderByDescending = false,Include includes = null, CancellationToken cancellationToken = default);
        
        int CountBy(ISpecification<T> specification);

        Task<int> CountByAsync(ISpecification<T> specification,
            CancellationToken cancellationToken = default);
        
        int CountBy<TKEntity>(ISpecification<TKEntity> specification) where TKEntity : class;

        Task<int> CountByAsync<TKEntity>(ISpecification<TKEntity> specification,CancellationToken cancellationToken = default) where TKEntity : class;
        
    }
}