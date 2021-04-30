// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data.Ado
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Innovt.Core.Collections;
using Innovt.Core.Cqrs.Queries;
using Innovt.Data.DataSources;
using Innovt.Data.Exceptions;
using Innovt.Data.Model;

namespace Innovt.Data.Ado
{
    public class RepositoryBase : IRepositoryBase
    {
        private readonly IConnectionFactory connectionFactory;
        private readonly IDataSource dataSource;

        public RepositoryBase(IDataSource datasource) : this(datasource, null)
        {
        }

        public RepositoryBase(IDataSource dataSource, IConnectionFactory connectionFactory)
        {
            this.dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));

            this.connectionFactory = connectionFactory ?? new ConnectionFactory();
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string tableName, string whereClause, object filter = null,
            CancellationToken cancellationToken = default, params string[] columns)
        {
            if (tableName == null) throw new ArgumentNullException(nameof(tableName));
            if (whereClause == null) throw new ArgumentNullException(nameof(whereClause));

            return QueryFirstOrDefaultInternalAsync<T>(tableName, whereClause, filter, cancellationToken, columns);
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object filter = null,
            CancellationToken cancellationToken = default)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            return QueryFirstOrDefaultInternalAsync<T>(sql, filter, cancellationToken);
        }

        public Task<T> QuerySingleOrDefaultAsync<T>(string sql, object filter = null,
            CancellationToken cancellationToken = default)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            return QuerySingleOrDefaultInternalAsync<T>(sql, filter, cancellationToken);
        }

        public Task<T> QuerySingleOrDefaultAsync<T>(string tableName, string whereClause, object filter = null,
            CancellationToken cancellationToken = default, params string[] columns)
        {
            var sql = $"SELECT {string.Join(",", columns)} FROM [{tableName}] WHERE {whereClause}";

            return QuerySingleOrDefaultInternalAsync<T>(sql, filter, cancellationToken);
        }

        public Task<int> QueryCountAsync(string tableName, string whereClause = null, object filter = null,
            CancellationToken cancellationToken = default)
        {
            if (tableName == null) throw new ArgumentNullException(nameof(tableName));

            return QueryCountInternalAsync(tableName, whereClause, filter, cancellationToken);
        }


        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object filter = null,
            CancellationToken cancellationToken = default)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            return QueryInternalAsync<T>(sql, filter, cancellationToken);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, object filter,
            Func<TFirst, TSecond, TReturn> func, string splitOn, CancellationToken cancellationToken = default)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            return QueryInternalAsync(sql, filter, func, splitOn, cancellationToken);
        }


        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, object filter,
            Func<TFirst, TSecond, TThird, TReturn> func, string splitOn, CancellationToken cancellationToken = default)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            return QueryInternalAsync(sql, filter, func, splitOn, cancellationToken);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql,
            object filter, Func<TFirst, TSecond, TThird, TFourth, TReturn> func, string splitOn,
            CancellationToken cancellationToken = default)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            return QueryInternalAsync(sql, filter, func, splitOn, cancellationToken);
        }

        public Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql,
            object filter, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> func, string splitOn,
            CancellationToken cancellationToken = default)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));

            return QueryInternalAsync(sql, filter, func, splitOn, cancellationToken);
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, object filter = null, IDbTransaction dbTransaction = null,
            CancellationToken cancellationToken = default)
        {
            if (sql is null) throw new ArgumentNullException(nameof(sql));


            return ExecuteInternalScalar<T>(sql, filter, dbTransaction, cancellationToken);
        }

        public Task<int> ExecuteAsync(string sql, object filter = null, IDbTransaction dbTransaction = null,
            CancellationToken cancellationToken = default)
        {
            if (sql is null) throw new ArgumentNullException(nameof(sql));

            return ExecuteInternalAsync(sql, filter, dbTransaction, cancellationToken);
        }
        
        public async Task<PagedCollection<T>> QueryPagedAsync<T>(string sql, IPagedFilter filter, bool useCount = true,CancellationToken cancellationToken = default) where T : class
        {
            var orderByIndex = sql.LastIndexOf("ORDER BY", StringComparison.CurrentCultureIgnoreCase);

            if (orderByIndex <= 0)
                throw new SqlSyntaxException("ORDER BY Clause not found. To filter as paged you need to provide an ORDER BY Clause.");

            var newSql = sql.Substring(0, orderByIndex);
            
            var queryPaged = sql.AddPagination(filter, dataSource);

            using var con = GetConnection();

            if (!useCount)
            {
                var pagedResult = await con.QueryAsync<T>(new CommandDefinition(queryPaged, filter, cancellationToken: cancellationToken));
                return new PagedCollection<T>(pagedResult, filter.Page, filter.PageSize);
            }

            var queryCount = $"SELECT COUNT(1) FROM ({newSql}) C";

            var totalRecords = 0;
            IEnumerable<T> queryResult = null;
            
            if (dataSource.Provider == Provider.Oracle)
            {
                totalRecords = await con.QuerySingleAsync<int>(new CommandDefinition(queryCount, filter, cancellationToken: cancellationToken))
                .ConfigureAwait(false);
                queryResult = await con
                    .QueryAsync<T>(new CommandDefinition(queryPaged, filter, cancellationToken: cancellationToken))
                    .ConfigureAwait(false);
            }
            else
            {
                var query = $"{queryCount}; {queryPaged}";
                var qResult = await con.QueryMultipleAsync(new CommandDefinition(query, filter, cancellationToken: cancellationToken));

                totalRecords = qResult.ReadFirst<int>();
                queryResult = await qResult.ReadAsync<T>().ConfigureAwait(false);
            }

            return new PagedCollection<T>(queryResult, filter.Page, filter.PageSize)
            { 
                TotalRecords = totalRecords
            };
        }

        public Task<IEnumerable<T>> QueryListPagedAsync<T>(string sql, IPagedFilter filter,
            CancellationToken cancellationToken = default)
        {
            if (sql == null) throw new ArgumentNullException(nameof(sql));
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            return QueryListPagedInternalAsync<T>(sql, filter, cancellationToken);
        }

        private IDbConnection GetConnection()
        {
            return connectionFactory.Create(dataSource);
        }

        private Task<T> QueryFirstOrDefaultInternalAsync<T>(string sql, object filter = null,
            CancellationToken cancellationToken = default)
        {
            using var con = GetConnection();

            var result = con
                .QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken));

            return result;
        }

        private Task<T> QueryFirstOrDefaultInternalAsync<T>(string tableName, string whereClause,
            object filter = null, CancellationToken cancellationToken = default, params string[] columns)
        {
            var fields = string.Join(",", columns);

            var sql = $"SELECT TOP 1 {fields} FROM [{tableName}] WHERE {whereClause}";

            return QueryFirstOrDefaultInternalAsync<T>(sql, filter, cancellationToken);
        }

        private Task<T> QuerySingleOrDefaultInternalAsync<T>(string sql, object filter = null,
            CancellationToken cancellationToken = default)
        {
            using var con = GetConnection();
            return con
                .QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken));
        }

        private Task<int> QueryCountInternalAsync(string tableName, string whereClause = null,
            object filter = null, CancellationToken cancellationToken = default)
        {
            var sql = $"SELECT COUNT(1) FROM [{tableName}]";

            if (whereClause.IsNotNullOrEmpty()) sql += $" WHERE {whereClause} ";

            return QuerySingleOrDefaultAsync<int>(sql, filter, cancellationToken);
        }

        private Task<IEnumerable<T>> QueryInternalAsync<T>(string sql, object filter = null,
            CancellationToken cancellationToken = default)
        {
            using var con = GetConnection();
            return con.QueryAsync<T>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken));
        }

        private Task<IEnumerable<TReturn>> QueryInternalAsync<TFirst, TSecond, TReturn>(string sql, object filter,
            Func<TFirst, TSecond, TReturn> func, string splitOn, CancellationToken cancellationToken = default)
        {
            using var con = GetConnection();
            return con.QueryAsync(
                new CommandDefinition(sql, filter, cancellationToken: cancellationToken), func, splitOn);
        }

        private Task<IEnumerable<TReturn>> QueryInternalAsync<TFirst, TSecond, TThird, TReturn>(string sql,
            object filter, Func<TFirst, TSecond, TThird, TReturn> func, string splitOn,
            CancellationToken cancellationToken = default)
        {
            using var con = GetConnection();
            return con.QueryAsync(
                new CommandDefinition(sql, filter, cancellationToken: cancellationToken), func, splitOn);
        }

        private Task<IEnumerable<TReturn>> QueryInternalAsync<TFirst, TSecond, TThird, TFourth, TReturn>(
            string sql, object filter, Func<TFirst, TSecond, TThird, TFourth, TReturn> func, string splitOn,
            CancellationToken cancellationToken = default)
        {
            using var con = GetConnection();
            return con.QueryAsync(
                new CommandDefinition(sql, filter, cancellationToken: cancellationToken), func, splitOn);
        }

        private Task<IEnumerable<TReturn>> QueryInternalAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            string sql, object filter, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> func, string splitOn,
            CancellationToken cancellationToken = default)
        {
            using var con = GetConnection();
            return con.QueryAsync(
                new CommandDefinition(sql, filter, cancellationToken: cancellationToken), func, splitOn);
        }

        private Task<T> ExecuteInternalScalar<T>(string sql, object filter = null,
            IDbTransaction dbTransaction = null, CancellationToken cancellationToken = default)
        {
            using var con = GetConnection();

            return con
                .ExecuteScalarAsync<T>(new CommandDefinition(sql, filter, dbTransaction,
                    cancellationToken: cancellationToken));
        }

        private Task<int> ExecuteInternalAsync(string sql, object filter = null,
            IDbTransaction dbTransaction = null, CancellationToken cancellationToken = default)
        {
            using var con = GetConnection();

            return con
                .ExecuteAsync(new CommandDefinition(sql, filter, dbTransaction, cancellationToken: cancellationToken));
        }

        private Task<IEnumerable<T>> QueryListPagedInternalAsync<T>(string sql, IPagedFilter filter,
            CancellationToken cancellationToken = default)
        {
            var query = sql.AddPagination(filter, dataSource);

            using var con = GetConnection();

            return con.QueryAsync<T>(new CommandDefinition(query, filter, cancellationToken: cancellationToken));
        }

        //private Task<SqlMapper.GridReader> QueryMultipleInternalAsync(IDbConnection con, string[] queries,
        //    object filter = null)
        //{
        //    if (queries == null) throw new ArgumentNullException(nameof(queries));

        //    var sql = new StringBuilder();

        //    foreach (var query in queries) sql.Append(query);

        //    return con.QueryMultipleAsync(new CommandDefinition(sql.ToString(), filter));
        //}

        //private async  Task<(IEnumerable<TFirst> firstCollection, IEnumerable<TSecond> secondCollection)> ReadMultipleInternalAsync<TFirst, TSecond>(string[] queries, object filter = null)
        //{
        //    using (var con = GetConnection())
        //    {
        //        var result = await QueryMultipleInternalAsync(con, queries, filter);

        //        return (await result.ReadAsync<TFirst>(), await result.ReadAsync<TSecond>());
        //    }
        //}

        //private async Task<IEnumerable<TReturn>> ReadMultipleInternalAsync<TFirst, TSecond, TThird, TReturn>(
        //    string[] queries, Func<TFirst, TSecond, TThird, TReturn> func, object filter = null, string splitOn = "id")
        //{
        //    using (var con = GetConnection())
        //    {
        //        var result = await QueryMultipleInternalAsync(con, queries, filter);

        //        return result.Read(func, splitOn, true).ToList();
        //    }
        //}

        //private async Task<IEnumerable<TReturn>> ReadMultipleInternalAsync<TFirst, TSecond, TThird, TFourth, TReturn>(
        //    string[] queries, Func<TFirst, TSecond, TThird, TFourth, TReturn> func, object filter = null,
        //    string splitOn = "id")
        //{
        //    using (var con = GetConnection())
        //    {
        //        var result = await QueryMultipleInternalAsync(con, queries, filter);

        //        return result.Read(func, splitOn, true).ToList();
        //    }
        //}
    }
}