using Dapper;
using Innovt.Core.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Queries;
using System.Threading;
using Innovt.Data.DataSources;
using Innovt.Data.Exceptions;

namespace Innovt.Data.Ado
{   
    public class RepositoryBase : IRepositoryBase
    {
        private readonly IDataSource dataSource;
        private readonly IConnectionFactory connectionFactory;

        public RepositoryBase(IDataSource datasource) : this(datasource, null)
        {
        }

        public RepositoryBase(IDataSource dataSource, IConnectionFactory connectionFactory)
        {
            this.dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));

            this.connectionFactory = connectionFactory ?? new ConnectionFactory();
        }

        private IDbConnection GetConnection()
        {
            return connectionFactory.Create(dataSource);
        }
        private async Task<T> QueryFirstOrDefaultInternalAsync<T>(string sql, object filter = null, CancellationToken cancellationToken = default)
        {  
            using var con = GetConnection();

            var result = await con.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken)).ConfigureAwait(false);
            return result;
        }

        private async Task<T> QueryFirstOrDefaultInternalAsync<T>(string tableName,string whereClause, object filter = null, CancellationToken cancellationToken = default,params string[] columns)
        {   
            var fields = string.Join(",", columns);

            var sql = $"SELECT TOP 1 {fields} FROM [{tableName}] WHERE {whereClause}";

            return await QueryFirstOrDefaultInternalAsync<T>(sql, filter, cancellationToken);
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string tableName,string whereClause, object filter = null, CancellationToken cancellationToken = default,params string[] columns)
        {
            if (tableName == null) throw new ArgumentNullException(nameof(tableName));
            if (whereClause == null) throw new ArgumentNullException(nameof(whereClause));

            return QueryFirstOrDefaultInternalAsync<T>(tableName, whereClause, filter, cancellationToken, columns);
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object filter = null, CancellationToken cancellationToken = default)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            return QueryFirstOrDefaultInternalAsync<T>(sql, filter, cancellationToken);
        }
        
        internal async Task<T> QuerySingleOrDefaultInternalAsync<T>(string sql, object filter = null, CancellationToken cancellationToken = default)
        { 
            using var con = GetConnection();
            return await con
                .QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken))
                .ConfigureAwait(false);
        }

        public Task<T> QuerySingleOrDefaultAsync<T>(string sql, object filter = null, CancellationToken cancellationToken = default)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));
           
            return QuerySingleOrDefaultInternalAsync<T>(sql,filter,cancellationToken);
        }

        public Task<T> QuerySingleOrDefaultAsync<T>(string tableName, string whereClause, object filter = null, CancellationToken cancellationToken = default, params string[] columns)
        {
            var sql = $"SELECT {string.Join(",", columns)} FROM [{tableName}] WHERE {whereClause}";

            return this.QuerySingleOrDefaultInternalAsync<T>(sql, filter, cancellationToken);
        }

        private async Task<int> QueryCountInternalAsync(string tableName, string whereClause = null, object filter = null, CancellationToken cancellationToken = default)
        {
            var sql = $"SELECT COUNT(1) FROM [{tableName}]";

            if (whereClause.IsNotNullOrEmpty())
            {
                sql += $" WHERE { whereClause} ";
            }

            return await this.QuerySingleOrDefaultAsync<int>(sql, filter, cancellationToken).ConfigureAwait(false);
        }
        
        public Task<int> QueryCountAsync(string tableName, string whereClause = null, object filter = null, CancellationToken cancellationToken = default)
        {
            if (tableName == null) throw new ArgumentNullException(nameof(tableName));

            return QueryCountInternalAsync(tableName, whereClause, filter, cancellationToken);
        }

        private async Task<IEnumerable<T>> QueryInternalAsync<T>(string sql, object filter=null, CancellationToken cancellationToken = default)
        {
            using var con = GetConnection();
            return await con.QueryAsync<T>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken))
                .ConfigureAwait(false);
        }

        private async Task<IEnumerable<TReturn>> QueryInternalAsync<TFirst, TSecond, TReturn>(string sql,object filter, Func<TFirst, TSecond, TReturn> func, string splitOn, CancellationToken cancellationToken = default)
        {
            using var con = GetConnection();
            return await con.QueryAsync<TFirst, TSecond, TReturn>(
                new CommandDefinition(sql, filter, cancellationToken: cancellationToken), func, splitOn);
        }

        private async Task<IEnumerable<TReturn>> QueryInternalAsync<TFirst, TSecond, TThird, TReturn>(string sql,object filter, Func<TFirst, TSecond,TThird, TReturn> func, string splitOn, CancellationToken cancellationToken = default)
        {
            using var con = GetConnection();
            return await con.QueryAsync<TFirst, TSecond, TThird,TReturn>(
                new CommandDefinition(sql, filter, cancellationToken: cancellationToken), func, splitOn);
        } 
        private async Task<IEnumerable<TReturn>> QueryInternalAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql,object filter, Func<TFirst, TSecond,TThird, TFourth, TReturn> func, string splitOn, CancellationToken cancellationToken = default)
        {
            using var con = GetConnection();
            return await con.QueryAsync<TFirst, TSecond, TThird,TFourth, TReturn>(
                new CommandDefinition(sql, filter, cancellationToken: cancellationToken), func, splitOn);
        }
        
        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object filter=null, CancellationToken cancellationToken = default)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            return this.QueryInternalAsync<T>(sql,filter,cancellationToken);
        }
        
        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, object filter, Func<TFirst, TSecond, TReturn> func, string splitOn, CancellationToken cancellationToken = default)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            return this.QueryInternalAsync(sql,filter,func,splitOn,cancellationToken);
        }
        
        
        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, object filter, Func<TFirst, TSecond, TThird, TReturn> func, string splitOn, CancellationToken cancellationToken = default)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            return this.QueryInternalAsync(sql,filter,func,splitOn,cancellationToken);
        }
        
        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, object filter, Func<TFirst, TSecond, TThird, TFourth, TReturn> func, string splitOn, CancellationToken cancellationToken = default)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            return this.QueryInternalAsync(sql,filter,func,splitOn,cancellationToken);
        }

        private async Task<T> ExecuteInternalScalar<T>(string sql, object filter = null, IDbTransaction dbTransaction=null, CancellationToken cancellationToken = default)
        {
            using var con = GetConnection();
            
            return await con.ExecuteScalarAsync<T>(new CommandDefinition(sql, filter, dbTransaction,cancellationToken: cancellationToken))
                .ConfigureAwait(false);
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, object filter = null, IDbTransaction dbTransaction = null, CancellationToken cancellationToken = default)
        {
            if (sql is null)
            {
                throw new ArgumentNullException(nameof(sql));
            };

            return this.ExecuteInternalScalar<T>(sql, filter, dbTransaction, cancellationToken);
        }
        private async Task<int> ExecuteInternalAsync(string sql, object filter = null, IDbTransaction dbTransaction = null, CancellationToken cancellationToken = default)
        {
            using var con = GetConnection();

            return await con.ExecuteAsync(new CommandDefinition(sql, filter, dbTransaction, cancellationToken: cancellationToken))
                .ConfigureAwait(false);
        }

        public Task<int> ExecuteAsync(string sql, object filter = null, IDbTransaction dbTransaction = null, CancellationToken cancellationToken = default)
        {
            if (sql is null)
            {
                throw new ArgumentNullException(nameof(sql));
            };

            return this.ExecuteInternalAsync(sql, filter, dbTransaction, cancellationToken);
        }


        public async Task<PagedCollection<T>> QueryPagedAsync<T>(string sql, IPagedFilter filter, CancellationToken cancellationToken = default) where T : class
        {
            var orderByIndex = sql.LastIndexOf("ORDER BY", StringComparison.CurrentCultureIgnoreCase);

            if (orderByIndex <= 0)
                throw new SqlSyntaxException("ORDER BY Clause not found. To filter as pagged you need to provide an ORDER BY Clause.");

            var newSql = sql.Substring(0, orderByIndex);

            var query = $"SELECT COUNT(1) FROM ({newSql}) C;" + sql.AddPagination(filter, dataSource);

            using var con = GetConnection();
            var qResult = await con.QueryMultipleAsync(new CommandDefinition(query, filter, cancellationToken: cancellationToken));

            int totalRecords = qResult.ReadFirst<int>();

            var result = new PagedCollection<T>(qResult.Read<T>(), filter.Page, filter.PageSize)
            { 
                TotalRecords = totalRecords
            };
            return result;
        }

        private async Task<IEnumerable<T>> QueryListPagedInternalAsync<T>(string sql, IPagedFilter filter, CancellationToken cancellationToken = default)
        {   
            var query = sql.AddPagination(filter,dataSource);

            using var con = GetConnection();

            return  await con.QueryAsync<T>(new CommandDefinition(query, filter, cancellationToken: cancellationToken)).ConfigureAwait(false);
        }

        public Task<IEnumerable<T>> QueryListPagedAsync<T>( string sql, IPagedFilter filter, CancellationToken cancellationToken = default)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            return QueryListPagedInternalAsync<T>(sql, filter, cancellationToken);
        }

        private async Task<SqlMapper.GridReader> QueryMultipleInternalAsync(string[] queries,object filter = null)
        {
            if (queries == null) throw new ArgumentNullException(nameof(queries));
            
            var sql =new StringBuilder();
            
            foreach (var query in queries)
            {  
                sql.Append(query);
            }

            using var con = GetConnection();
            
            return  await con.QueryMultipleAsync(new CommandDefinition( sql.ToString(),filter)).ConfigureAwait(false);
        }

        private async Task<IEnumerable<TReturn>> ReadMultipleInternalAsync<TFirst, TSecond, TReturn>(string[] queries, Func<TFirst, TSecond, TReturn> func,object filter = null, string splitOn = "id")
        {
            var result = await QueryMultipleInternalAsync(queries,filter);

            return result.Read(func, splitOn, true).ToList();
        }
        
        private async Task<IEnumerable<TReturn>> ReadMultipleInternalAsync<TFirst, TSecond,TThird, TReturn>(string[] queries, Func<TFirst, TSecond,TThird, TReturn> func,object filter = null, string splitOn = "id")
        {
            var result = await QueryMultipleInternalAsync(queries,filter);

            return result.Read(func, splitOn, true).ToList();
        }
        
        private async Task<IEnumerable<TReturn>> ReadMultipleInternalAsync<TFirst, TSecond,TThird,TFourth, TReturn>(string[] queries, Func<TFirst, TSecond,TThird,TFourth, TReturn> func,object filter = null, string splitOn = "id")
        {
            var result = await QueryMultipleInternalAsync(queries, filter);

            return result.Read(func, splitOn, true).ToList();
        }

        public Task<IEnumerable<TReturn>> QueryMultipleAsync<TFirst, TSecond, TReturn>(string[] queries, Func<TFirst, TSecond, TReturn> func,object filter = null, string splitOn = "id")
        {
            if (queries == null) throw new ArgumentNullException(nameof(queries));
            if (func == null) throw new ArgumentNullException(nameof(func));

            return ReadMultipleInternalAsync(queries,func,filter, splitOn);
        }
        
        public Task<IEnumerable<TReturn>> QueryMultipleAsync<TFirst, TSecond,TThird, TReturn>(string[] queries, Func<TFirst, TSecond,TThird, TReturn> func,object filter = null, string splitOn = "id")
        {
            if (queries == null) throw new ArgumentNullException(nameof(queries));
            if (func == null) throw new ArgumentNullException(nameof(func));

            return ReadMultipleInternalAsync(queries, func,filter,splitOn);
        }
        
        public Task<IEnumerable<TReturn>> QueryMultipleAsync<TFirst, TSecond,TThird,TFourth, TReturn>(string[] queries, Func<TFirst, TSecond,TThird,TFourth, TReturn> func, object filter = null, string splitOn = "id")
        {
            if (queries == null) throw new ArgumentNullException(nameof(queries));
            if (func == null) throw new ArgumentNullException(nameof(func));

            return ReadMultipleInternalAsync(queries, func,filter,splitOn);
        }
    }
}
