using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Collections;
using Innovt.Core.Cqrs.Queries;

namespace Innovt.Data.Ado
{
    public interface IRepositoryBase
    {
        Task<T> QueryFirstOrDefaultAsync<T>(string tableName,string whereClause, object filter = null, CancellationToken cancellationToken = default,params string[] columns);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object filter = null, CancellationToken cancellationToken = default);
        Task<T> QuerySingleOrDefaultAsync<T>(string sql, object filter = null, CancellationToken cancellationToken = default);
        Task<T> QuerySingleOrDefaultAsync<T>(string tableName, string whereClause, object filter = null, CancellationToken cancellationToken = default, params string[] columns);
        Task<int> QueryCountAsync(string tableName, string whereClause = null, object filter = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object filter=null, CancellationToken cancellationToken = default);
        Task<PagedCollection<T>> QueryPagedAsync<T>(string sql, IPagedFilter filter,CancellationToken cancellationToken = default) where T : class;
        Task<IEnumerable<T>> QueryListPagedAsync<T>( string sql, IPagedFilter filter, CancellationToken cancellationToken = default);
        Task<IEnumerable<TReturn>> QueryMultipleAsync<TFirst, TSecond, TReturn>(string[] queries, Func<TFirst, TSecond, TReturn> func, string splitOn = "id");
    }
}