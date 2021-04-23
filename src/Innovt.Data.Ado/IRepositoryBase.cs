using System;
using System.Collections.Generic;
using System.Data;
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

        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, object filter,
            Func<TFirst, TSecond, TReturn> func, string splitOn, CancellationToken cancellationToken = default);


        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, object filter,
            Func<TFirst, TSecond, TThird, TReturn> func, string splitOn, CancellationToken cancellationToken = default);

        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, object filter,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> func, string splitOn,
            CancellationToken cancellationToken = default);
        
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth,TFifth, TReturn>(string sql, object filter,
            Func<TFirst, TSecond, TThird, TFourth,TFifth, TReturn> func, string splitOn,
            CancellationToken cancellationToken = default);
        
        Task<PagedCollection<T>> QueryPagedAsync<T>(string sql, IPagedFilter filter, bool useCount = true,CancellationToken cancellationToken = default) where T : class;

        Task<IEnumerable<T>> QueryListPagedAsync<T>( string sql, IPagedFilter filter, CancellationToken cancellationToken = default);
        // Task<IEnumerable<TReturn>> QueryMultipleAsync<TFirst, TSecond, TReturn>(string[] queries, Func<TFirst, TSecond, TReturn> func,object filter = null, string splitOn = "id");
        //
        // Task<IEnumerable<TReturn>> QueryMultipleAsync<TFirst, TSecond, TThird, TReturn>(string[] queries,
        //     Func<TFirst, TSecond, TThird, TReturn> func,object filter = null, string splitOn = "id");
        //
        // Task<IEnumerable<TReturn>> QueryMultipleAsync<TFirst, TSecond, TThird,TFourth, TReturn>(string[] queries,
        //     Func<TFirst, TSecond, TThird,TFourth, TReturn> func, object filter = null, string splitOn = "id");
        
        Task<T> ExecuteScalarAsync<T>(string sql, object filter = null, IDbTransaction dbTransaction = null, CancellationToken cancellationToken = default);

        Task<int> ExecuteAsync(string sql, object filter = null, IDbTransaction dbTransaction = null, CancellationToken cancellationToken = default);
        
    }
}