// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data.Ado
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Dapper;
using Innovt.Core.Collections;
using Innovt.Core.Cqrs.Queries;
using Innovt.Data.DataSources;
using Innovt.Data.Exceptions;
using Innovt.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Data.Ado;

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

    public async Task<T> QueryFirstOrDefaultAsync<T>(string tableName, string whereClause, object filter = null,
        CancellationToken cancellationToken = default, params string[] columns)
    {
        if (tableName == null) throw new ArgumentNullException(nameof(tableName));
        if (whereClause == null) throw new ArgumentNullException(nameof(whereClause));

        return await QueryFirstOrDefaultInternalAsync<T>(tableName, whereClause, filter, cancellationToken,
            columns).ConfigureAwait(false);
    }

    public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object filter = null,
        CancellationToken cancellationToken = default)
    {
        if (sql == null) throw new ArgumentNullException(nameof(sql));

        return await QueryFirstOrDefaultInternalAsync<T>(sql, filter, cancellationToken).ConfigureAwait(false);
    }

    public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object filter = null,
        CancellationToken cancellationToken = default)
    {
        if (sql == null) throw new ArgumentNullException(nameof(sql));

        return await QuerySingleOrDefaultInternalAsync<T>(sql, filter, cancellationToken).ConfigureAwait(false);
    }

    public async Task<T> QuerySingleOrDefaultAsync<T>(string tableName, string whereClause, object filter = null,
        CancellationToken cancellationToken = default, params string[] columns)
    {
        var sql = $"SELECT {string.Join(",", columns)} FROM [{tableName}]".AddNoLock(dataSource).AddWhere(whereClause);

        return await QuerySingleOrDefaultInternalAsync<T>(sql, filter, cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> QueryCountAsync(string tableName, string whereClause = null, object filter = null,
        CancellationToken cancellationToken = default)
    {
        if (tableName == null) throw new ArgumentNullException(nameof(tableName));

        return await QueryCountInternalAsync(tableName, whereClause, filter, cancellationToken)
            .ConfigureAwait(false);
    }


    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object filter = null,
        CancellationToken cancellationToken = default)
    {
        if (sql == null) throw new ArgumentNullException(nameof(sql));

        return await QueryInternalAsync<T>(sql, filter, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, object filter,
        Func<TFirst, TSecond, TReturn> func, string splitOn, CancellationToken cancellationToken = default)
    {
        if (sql == null) throw new ArgumentNullException(nameof(sql));

        return await QueryInternalAsync(sql, filter, func, splitOn, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, object filter,
        Func<TFirst, TSecond, TThird, TReturn> func, string splitOn, CancellationToken cancellationToken = default)
    {
        if (sql == null) throw new ArgumentNullException(nameof(sql));

        return await QueryInternalAsync(sql, filter, func, splitOn, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql,
        object filter, Func<TFirst, TSecond, TThird, TFourth, TReturn> func, string splitOn,
        CancellationToken cancellationToken = default)
    {
        if (sql == null) throw new ArgumentNullException(nameof(sql));

        return await QueryInternalAsync(sql, filter, func, splitOn, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
        string sql,
        object filter, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> func, string splitOn,
        CancellationToken cancellationToken = default)
    {
        if (sql == null) throw new ArgumentNullException(nameof(sql));

        return await QueryInternalAsync(sql, filter, func, splitOn, cancellationToken).ConfigureAwait(false);
    }

    public async Task<T> ExecuteScalarAsync<T>(string sql, object filter = null,
        IDbTransaction dbTransaction = null,
        CancellationToken cancellationToken = default)
    {
        if (sql is null) throw new ArgumentNullException(nameof(sql));


        return await ExecuteInternalScalar<T>(sql, filter, dbTransaction, cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> ExecuteAsync(string sql, object filter = null, IDbTransaction dbTransaction = null,
        CancellationToken cancellationToken = default)
    {
        if (sql is null) throw new ArgumentNullException(nameof(sql));

        return await ExecuteInternalAsync(sql, filter, dbTransaction, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PagedCollection<T>> QueryPagedAsync<T>(string sql, IPagedFilter filter, bool useCount = true,
        CancellationToken cancellationToken = default) where T : class
    {
        if (sql == null) throw new ArgumentNullException(nameof(sql));

        var orderByIndex = sql.LastIndexOf("ORDER BY", StringComparison.CurrentCultureIgnoreCase);

        if (orderByIndex <= 0)
            throw new SqlSyntaxException(
                "ORDER BY Clause not found. To filter as paged you need to provide an ORDER BY Clause.");

        var newSql = sql.Substring(0, orderByIndex);

        var queryPaged = sql.AddPagination(filter, dataSource);

        using var con = GetConnection();

        if (!useCount)
        {
            var pagedResult =
                await con.QueryAsync<T>(new CommandDefinition(queryPaged, filter,
                    cancellationToken: cancellationToken)).ConfigureAwait(false);
            return new PagedCollection<T>(pagedResult, filter.Page, filter.PageSize);
        }

        var queryCount = $"SELECT COUNT(1) FROM ({newSql}) AS INNOVT_COUNT";

        var totalRecords = 0;
        IEnumerable<T> queryResult = null;

        if (dataSource.Provider == Provider.Oracle)
        {
            totalRecords = await con
                .QuerySingleAsync<int>(new CommandDefinition(queryCount, filter,
                    cancellationToken: cancellationToken))
                .ConfigureAwait(false);
            queryResult = await con
                .QueryAsync<T>(new CommandDefinition(queryPaged, filter, cancellationToken: cancellationToken))
                .ConfigureAwait(false);
        }
        else
        {
            var query = $"{queryCount}; {queryPaged}";
            var qResult = await con.QueryMultipleAsync(new CommandDefinition(query, filter,
                cancellationToken: cancellationToken)).ConfigureAwait(false);

            totalRecords = await qResult.ReadFirstAsync<int>().ConfigureAwait(false);
            queryResult = await qResult.ReadAsync<T>().ConfigureAwait(false);
        }

        return new PagedCollection<T>(queryResult, filter.Page, filter.PageSize)
        {
            TotalRecords = totalRecords
        };
    }

    public async Task<IEnumerable<T>> QueryListPagedAsync<T>(string sql, IPagedFilter filter,
        CancellationToken cancellationToken = default)
    {
        if (sql == null) throw new ArgumentNullException(nameof(sql));
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return await QueryListPagedInternalAsync<T>(sql, filter, cancellationToken).ConfigureAwait(false);
    }

    private IDbConnection GetConnection()
    {
        return connectionFactory.Create(dataSource);
    }

    private async Task<T> QueryFirstOrDefaultInternalAsync<T>(string sql, object filter = null,
        CancellationToken cancellationToken = default)
    {
        using var con = GetConnection();

        return await con
            .QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken))
            .ConfigureAwait(false);
    }

    private async Task<T> QueryFirstOrDefaultInternalAsync<T>(string tableName, string whereClause,
        object filter = null, CancellationToken cancellationToken = default, params string[] columns)
    {
        var fields = string.Join(",", columns);

        var sql = $"SELECT TOP 1 {fields} FROM [{tableName}] ".AddNoLock(dataSource).AddWhere(whereClause);

        return await QueryFirstOrDefaultInternalAsync<T>(sql, filter, cancellationToken).ConfigureAwait(false);
    }

    private async Task<T> QuerySingleOrDefaultInternalAsync<T>(string sql, object filter = null,
        CancellationToken cancellationToken = default)
    {
        using var con = GetConnection();
        return await con
            .QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken))
            .ConfigureAwait(false);
    }

    private async Task<int> QueryCountInternalAsync(string tableName, string whereClause = null,
        object filter = null, CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT COUNT(1) FROM [{tableName}] ".AddNoLock(dataSource).AddWhere(whereClause);

        return await QuerySingleOrDefaultAsync<int>(sql, filter, cancellationToken).ConfigureAwait(false);
    }

    private async Task<IEnumerable<T>> QueryInternalAsync<T>(string sql, object filter = null,
        CancellationToken cancellationToken = default)
    {
        using var con = GetConnection();
        return await con.QueryAsync<T>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken))
            .ConfigureAwait(false);
    }

    private async Task<IEnumerable<TReturn>> QueryInternalAsync<TFirst, TSecond, TReturn>(string sql, object filter,
        Func<TFirst, TSecond, TReturn> func, string splitOn, CancellationToken cancellationToken = default)
    {
        using var con = GetConnection();
        return await con
            .QueryAsync(new CommandDefinition(sql, filter, cancellationToken: cancellationToken), func, splitOn)
            .ConfigureAwait(false);
    }

    private async Task<IEnumerable<TReturn>> QueryInternalAsync<TFirst, TSecond, TThird, TReturn>(string sql,
        object filter, Func<TFirst, TSecond, TThird, TReturn> func, string splitOn,
        CancellationToken cancellationToken = default)
    {
        using var con = GetConnection();
        return await con.QueryAsync(new CommandDefinition(sql, filter, cancellationToken: cancellationToken), func,
            splitOn).ConfigureAwait(false);
    }

    private async Task<IEnumerable<TReturn>> QueryInternalAsync<TFirst, TSecond, TThird, TFourth, TReturn>(
        string sql, object filter, Func<TFirst, TSecond, TThird, TFourth, TReturn> func, string splitOn,
        CancellationToken cancellationToken = default)
    {
        using var con = GetConnection();
        return await con.QueryAsync(
                new CommandDefinition(sql, filter, cancellationToken: cancellationToken), func, splitOn)
            .ConfigureAwait(false);
    }

    private async Task<IEnumerable<TReturn>> QueryInternalAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
        string sql, object filter, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> func, string splitOn,
        CancellationToken cancellationToken = default)
    {
        using var con = GetConnection();
        return await con.QueryAsync(
                new CommandDefinition(sql, filter, cancellationToken: cancellationToken), func, splitOn)
            .ConfigureAwait(false);
    }

    private async Task<T> ExecuteInternalScalar<T>(string sql, object filter = null,
        IDbTransaction dbTransaction = null, CancellationToken cancellationToken = default)
    {
        using var con = GetConnection();

        return await con
            .ExecuteScalarAsync<T>(new CommandDefinition(sql, filter, dbTransaction,
                cancellationToken: cancellationToken)).ConfigureAwait(false);
    }

    private async Task<int> ExecuteInternalAsync(string sql, object filter = null,
        IDbTransaction dbTransaction = null, CancellationToken cancellationToken = default)
    {
        using var con = GetConnection();

        return await con
            .ExecuteAsync(new CommandDefinition(sql, filter, dbTransaction, cancellationToken: cancellationToken))
            .ConfigureAwait(false);
    }

    private async Task<IEnumerable<T>> QueryListPagedInternalAsync<T>(string sql, IPagedFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = sql.AddPagination(filter, dataSource);

        using var con = GetConnection();

        return await con.QueryAsync<T>(new CommandDefinition(query, filter, cancellationToken: cancellationToken))
            .ConfigureAwait(false);
    }
}