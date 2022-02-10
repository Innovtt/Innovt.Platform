// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Domain.Core.Repository
{
    public interface IExtendedUnitOfWork : IUnitOfWork
    {
        void Add<T>(T entity) where T : class;

        void Add<T>(IEnumerable<T> entities) where T : class;

        Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;

        Task AddAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class;

        void Remove<T>(T entity) where T : class;

        void Remove<T>(IEnumerable<T> entities) where T : class;

        void Update<T>(T entity) where T : class;

        void Attach<T>(T entity) where T : class;

        void Detach<T>(T entity) where T : class;
        
        int ExecuteSqlCommand(string sql, params object[] parameters);

        Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken = default, params object[] parameters);

        System.Linq.IQueryable<T> Queryable<T>() where T : class;
    }
}