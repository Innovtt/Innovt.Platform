// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.Ado

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

namespace Innovt.Data.Ado;
/// <summary>
/// Represents a base repository implementation that provides common data access methods.
/// </summary>
public class RepositoryBase : IRepositoryBase
{
    private readonly IConnectionFactory connectionFactory;
    private readonly IDataSource dataSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase"/> class with the specified data source.
    /// </summary>
    /// <param name="dataSource">The data source to use for database operations.</param>
    public RepositoryBase(IDataSource datasource) : this(datasource, null)
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase"/> class with the specified data source and connection factory.
    /// </summary>
    /// <param name="dataSource">The data source to use for database operations.</param>
    /// <param name="connectionFactory">The connection factory for creating database connections.</param>
    public RepositoryBase(IDataSource dataSource, IConnectionFactory connectionFactory)
    {
        this.dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        this.connectionFactory = connectionFactory ?? new ConnectionFactory();
    }
    /// <summary>
    /// Asynchronously retrieves the first result of a query from the specified table based on the provided where clause and optional columns.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="whereClause">The WHERE clause for the query.</param>
    /// <param name="filter">Additional filter parameters for the query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="columns">The optional columns to select.</param>
    /// <returns>The first result of the query.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="tableName"/> or <paramref name="whereClause"/> is null.</exception>
    public async Task<T> QueryFirstOrDefaultAsync<T>(string tableName, string whereClause, object filter = null,
        CancellationToken cancellationToken = default, params string[] columns)
    {
        if (tableName == null) throw new ArgumentNullException(nameof(tableName));
        if (whereClause == null) throw new ArgumentNullException(nameof(whereClause));

        return await QueryFirstOrDefaultInternalAsync<T>(tableName, whereClause, filter, cancellationToken,
            columns).ConfigureAwait(false);
    }
    /// <summary>
    /// Asynchronously retrieves the first result of a query using the provided SQL statement.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="sql">The SQL statement for the query.</param>
    /// <param name="filter">Additional filter parameters for the query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The first result of the query.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="sql"/> is null.</exception>
    public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object filter = null,
        CancellationToken cancellationToken = default)
    {
        if (sql == null) throw new ArgumentNullException(nameof(sql));

        return await QueryFirstOrDefaultInternalAsync<T>(sql, filter, cancellationToken).ConfigureAwait(false);
    }
    /// <summary>
    /// Asynchronously executes a SQL query and returns a single result of type T, or a default value if no result is found.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="filter">An optional object that can be used to pass parameters or filters to the SQL query.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task will return the single result of type T if found,
    /// or the default value for type T if no result is found.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the 'sql' parameter is null.</exception>
    public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object filter = null,
        CancellationToken cancellationToken = default)
    {
        if (sql == null) throw new ArgumentNullException(nameof(sql));

        return await QuerySingleOrDefaultInternalAsync<T>(sql, filter, cancellationToken).ConfigureAwait(false);
    }
    /// <summary>
    /// Asynchronously retrieves a single result from the specified table based on the provided where clause and optional columns.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="whereClause">The WHERE clause for the query.</param>
    /// <param name="filter">Additional filter parameters for the query.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="columns">The optional columns to select.</param>
    /// <returns>A single result from the query.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="tableName"/> or <paramref name="whereClause"/> is null.</exception>
    public async Task<T> QuerySingleOrDefaultAsync<T>(string tableName, string whereClause, object filter = null,
        CancellationToken cancellationToken = default, params string[] columns)
    {
        var sql = $"SELECT {string.Join(",", columns)} FROM [{tableName}]".AddNoLock(dataSource).AddWhere(whereClause);

        return await QuerySingleOrDefaultInternalAsync<T>(sql, filter, cancellationToken).ConfigureAwait(false);
    }
    /// <summary>
    /// Asynchronously executes a SQL query to count records in a specified table.
    /// </summary>
    /// <param name="tableName">The name of the table to query.</param>
    /// <param name="whereClause">An optional WHERE clause to filter the count.</param>
    /// <param name="filter">An optional object that can be used to pass parameters or filters to the SQL query.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task will return the count of records based on the provided criteria.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the 'tableName' parameter is null.</exception>
    public async Task<int> QueryCountAsync(string tableName, string whereClause = null, object filter = null,
        CancellationToken cancellationToken = default)
    {
        if (tableName == null) throw new ArgumentNullException(nameof(tableName));

        return await QueryCountInternalAsync(tableName, whereClause, filter, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously executes a SQL query and returns a collection of results of type T.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="filter">An optional object that can be used to pass parameters or filters to the SQL query.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task will return a collection of results of type T based on the SQL query.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the 'sql' parameter is null.</exception>
    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object filter = null,
        CancellationToken cancellationToken = default)
    {
        if (sql == null) throw new ArgumentNullException(nameof(sql));

        return await QueryInternalAsync<T>(sql, filter, cancellationToken).ConfigureAwait(false);
    }
    /// <summary>
    /// Asynchronously executes a SQL query and returns a collection of results by mapping to a custom type using a provided function.
    /// </summary>
    /// <typeparam name="TFirst">The type of the first result.</typeparam>
    /// <typeparam name="TSecond">The type of the second result.</typeparam>
    /// <typeparam name="TReturn">The type to map the results to.</typeparam>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="filter">An optional object that can be used to pass parameters or filters to the SQL query.</param>
    /// <param name="func">A function to map the query results to the desired type.</param>
    /// <param name="splitOn">The column name to split the results on.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task will return a collection of results of the specified mapped type.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the 'sql' parameter is null.</exception>
    public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, object filter,
        Func<TFirst, TSecond, TReturn> func, string splitOn, CancellationToken cancellationToken = default)
    {
        if (sql == null) throw new ArgumentNullException(nameof(sql));

        return await QueryInternalAsync(sql, filter, func, splitOn, cancellationToken).ConfigureAwait(false);
    }
    /// <summary>
    /// Asynchronously executes a SQL query and returns a collection of results by mapping to a custom type using a provided function.
    /// </summary>
    /// <typeparam name="TFirst">The type of the first result.</typeparam>
    /// <typeparam name="TSecond">The type of the second result.</typeparam>
    /// <typeparam name="TThird">The type of the third result.</typeparam>
    /// <typeparam name="TReturn">The type to map the results to.</typeparam>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="filter">An optional object that can be used to pass parameters or filters to the SQL query.</param>
    /// <param name="func">A function to map the query results to the desired type.</param>
    /// <param name="splitOn">The column name to split the results on.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task will return a collection of results of the specified mapped type.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the 'sql' parameter is null.</exception>
    public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, object filter,
        Func<TFirst, TSecond, TThird, TReturn> func, string splitOn, CancellationToken cancellationToken = default)
    {
        if (sql == null) throw new ArgumentNullException(nameof(sql));

        return await QueryInternalAsync(sql, filter, func, splitOn, cancellationToken).ConfigureAwait(false);
    }
    /// <summary>
    /// Asynchronously executes a SQL query and returns a collection of results by mapping to a custom type using a provided function.
    /// </summary>
    /// <typeparam name="TFirst">The type of the first result.</typeparam>
    /// <typeparam name="TSecond">The type of the second result.</typeparam>
    /// <typeparam name="TThird">The type of the third result.</typeparam>
    /// <typeparam name="TFourth">The type of the fourth result.</typeparam>
    /// <typeparam name="TReturn">The type to map the results to.</typeparam>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="filter">An optional object that can be used to pass parameters or filters to the SQL query.</param>
    /// <param name="func">A function to map the query results to the desired type.</param>
    /// <param name="splitOn">The column name to split the results on.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task will return a collection of results of the specified mapped type.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the 'sql' parameter is null.</exception>
    public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql,
        object filter, Func<TFirst, TSecond, TThird, TFourth, TReturn> func, string splitOn,
        CancellationToken cancellationToken = default)
    {
        if (sql == null) throw new ArgumentNullException(nameof(sql));

        return await QueryInternalAsync(sql, filter, func, splitOn, cancellationToken).ConfigureAwait(false);
    }
    /// <summary>
    /// Asynchronously executes a SQL query and returns a collection of results by mapping to a custom type using a provided function.
    /// </summary>
    /// <typeparam name="TFirst">The type of the first result.</typeparam>
    /// <typeparam name="TSecond">The type of the second result.</typeparam>
    /// <typeparam name="TThird">The type of the third result.</typeparam>
    /// <typeparam name="TFourth">The type of the fourth result.</typeparam>
    /// <typeparam name="TFifth">The type of the fifth result.</typeparam>
    /// <typeparam name="TReturn">The type to map the results to.</typeparam>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="filter">An optional object that can be used to pass parameters or filters to the SQL query.</param>
    /// <param name="func">A function to map the query results to the desired type.</param>
    /// <param name="splitOn">The column name to split the results on.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task will return a collection of results of the specified mapped type.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the 'sql' parameter is null.</exception>
    public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
        string sql,
        object filter, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> func, string splitOn,
        CancellationToken cancellationToken = default)
    {
        if (sql == null) throw new ArgumentNullException(nameof(sql));

        return await QueryInternalAsync(sql, filter, func, splitOn, cancellationToken).ConfigureAwait(false);
    }
    /// <summary>
    /// Asynchronously executes a SQL command and returns a scalar result of type T.
    /// </summary>
    /// <typeparam name="T">The type of the scalar result.</typeparam>
    /// <param name="sql">The SQL command to execute.</param>
    /// <param name="filter">An optional object that can be used to pass parameters or filters to the SQL command.</param>
    /// <param name="dbTransaction">An optional database transaction for the command.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task will return the scalar result of type T from the executed SQL command.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the 'sql' parameter is null.</exception>
    public async Task<T> ExecuteScalarAsync<T>(string sql, object filter = null,
        IDbTransaction dbTransaction = null,
        CancellationToken cancellationToken = default)
    {
        if (sql is null) throw new ArgumentNullException(nameof(sql));


        return await ExecuteInternalScalar<T>(sql, filter, dbTransaction, cancellationToken).ConfigureAwait(false);
    }
    /// <summary>
    /// Asynchronously executes a SQL command and returns the number of affected rows.
    /// </summary>
    /// <param name="sql">The SQL command to execute.</param>
    /// <param name="filter">An optional object that can be used to pass parameters or filters to the SQL command.</param>
    /// <param name="dbTransaction">An optional database transaction for the command.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task will return the number of affected rows by the executed SQL command.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if the 'sql' parameter is null.</exception>
    public async Task<int> ExecuteAsync(string sql, object filter = null, IDbTransaction dbTransaction = null,
        CancellationToken cancellationToken = default)
    {
        if (sql is null) throw new ArgumentNullException(nameof(sql));

        return await ExecuteInternalAsync(sql, filter, dbTransaction, cancellationToken).ConfigureAwait(false);
    }
    /// <summary>
    /// Executes a paged SQL query asynchronously and returns the results as a PagedCollection.
    /// </summary>
    /// <typeparam name="T">The type of object to retrieve from the query.</typeparam>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="filter">The filter for pagination and ordering.</param>
    /// <param name="useCount">Flag indicating whether to include a total count of records. Defaults to true.</param>
    /// <param name="cancellationToken">A CancellationToken to cancel the asynchronous operation. Defaults to CancellationToken.None.</param>
    /// <returns>A PagedCollection containing the results of the query.</returns>
    /// <exception cref="ArgumentNullException">Thrown when sql is null.</exception>
    /// <exception cref="SqlSyntaxException">Thrown when the ORDER BY clause is not found in the SQL query.</exception>
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
    /// <summary>
    /// Executes a paged SQL query asynchronously and returns the results as an IEnumerable.
    /// </summary>
    /// <typeparam name="T">The type of object to retrieve from the query.</typeparam>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="filter">The filter for pagination and ordering.</param>
    /// <param name="cancellationToken">A CancellationToken to cancel the asynchronous operation. Defaults to CancellationToken.None.</param>
    /// <returns>An IEnumerable containing the results of the query.</returns>
    /// <exception cref="ArgumentNullException">Thrown when sql or filter is null.</exception>
    public async Task<IEnumerable<T>> QueryListPagedAsync<T>(string sql, IPagedFilter filter,
        CancellationToken cancellationToken = default)
    {
        if (sql == null) throw new ArgumentNullException(nameof(sql));
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return await QueryListPagedInternalAsync<T>(sql, filter, cancellationToken).ConfigureAwait(false);
    }
    /// <summary>
    /// Retrieves a database connection for executing queries.
    /// </summary>
    /// <returns>An IDbConnection instance for executing queries.</returns>
    private IDbConnection GetConnection()
    {
        return connectionFactory.Create(dataSource);
    }
    /// <summary>
    /// Executes a SQL query asynchronously and returns the first result or default value of type T.
    /// </summary>
    /// <typeparam name="T">The type of object to retrieve from the query.</typeparam>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="filter">The filter for the query. Defaults to null.</param>
    /// <param name="cancellationToken">A CancellationToken to cancel the asynchronous operation. Defaults to CancellationToken.None.</param>
    /// <returns>The first result of type T from the query, or default(T) if no result is found.</returns>
    private async Task<T> QueryFirstOrDefaultInternalAsync<T>(string sql, object filter = null,
        CancellationToken cancellationToken = default)
    {
        using var con = GetConnection();

        return await con
            .QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken))
            .ConfigureAwait(false);
    }
    /// <summary>
    /// Executes a SQL query asynchronously and returns the first result or default value of type T.
    /// </summary>
    /// <typeparam name="T">The type of object to retrieve from the query.</typeparam>
    /// <param name="tableName">The name of the database table.</param>
    /// <param name="whereClause">The WHERE clause for the query.</param>
    /// <param name="filter">The filter for the query. Defaults to null.</param>
    /// <param name="cancellationToken">A CancellationToken to cancel the asynchronous operation. Defaults to CancellationToken.None.</param>
    /// <param name="columns">The columns to select. Defaults to an empty array.</param>
    /// <returns>The first result of type T from the query, or default(T) if no result is found.</returns>
    private async Task<T> QueryFirstOrDefaultInternalAsync<T>(string tableName, string whereClause,
        object filter = null, CancellationToken cancellationToken = default, params string[] columns)
    {
        var fields = string.Join(",", columns);

        var sql = $"SELECT TOP 1 {fields} FROM [{tableName}] ".AddNoLock(dataSource).AddWhere(whereClause);

        return await QueryFirstOrDefaultInternalAsync<T>(sql, filter, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes a SQL query asynchronously and returns a single result or default value of type T.
    /// </summary>
    /// <typeparam name="T">The type of object to retrieve from the query.</typeparam>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="filter">The filter for the query. Defaults to null.</param>
    /// <param name="cancellationToken">A CancellationToken to cancel the asynchronous operation. Defaults to CancellationToken.None.</param>
    /// <returns>The single result of type T from the query, or default(T) if no result is found.</returns>
    private async Task<T> QuerySingleOrDefaultInternalAsync<T>(string sql, object filter = null,
        CancellationToken cancellationToken = default)
    {
        using var con = GetConnection();
        return await con
            .QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken))
            .ConfigureAwait(false);
    }
    /// <summary>
    /// Executes a SQL query asynchronously and returns the count of records.
    /// </summary>
    /// <param name="tableName">The name of the database table.</param>
    /// <param name="whereClause">The WHERE clause for the query. Defaults to null.</param>
    /// <param name="filter">The filter for the query. Defaults to null.</param>
    /// <param name="cancellationToken">A CancellationToken to cancel the asynchronous operation. Defaults to CancellationToken.None.</param>
    /// <returns>The count of records based on the provided criteria.</returns>
    private async Task<int> QueryCountInternalAsync(string tableName, string whereClause = null,
        object filter = null, CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT COUNT(1) FROM [{tableName}] ".AddNoLock(dataSource).AddWhere(whereClause);

        return await QuerySingleOrDefaultAsync<int>(sql, filter, cancellationToken).ConfigureAwait(false);
    }
    /// <summary>
    /// Executes a SQL query asynchronously and returns the results as an IEnumerable.
    /// </summary>
    /// <typeparam name="T">The type of object to retrieve from the query.</typeparam>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="filter">The filter for the query. Defaults to null.</param>
    /// <param name="cancellationToken">A CancellationToken to cancel the asynchronous operation. Defaults to CancellationToken.None.</param>
    /// <returns>An IEnumerable containing the results of the query.</returns>
    private async Task<IEnumerable<T>> QueryInternalAsync<T>(string sql, object filter = null,
        CancellationToken cancellationToken = default)
    {
        using var con = GetConnection();
        return await con.QueryAsync<T>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken))
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Executes a SQL query asynchronously and returns the results as an IEnumerable using a specified mapping function.
    /// </summary>
    /// <typeparam name="TFirst">The type of the first result set.</typeparam>
    /// <typeparam name="TSecond">The type of the second result set.</typeparam>
    /// <typeparam name="TReturn">The type of the resulting objects after mapping.</typeparam>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="filter">The filter for the query. Defaults to null.</param>
    /// <param name="func">A mapping function for the query results.</param>
    /// <param name="splitOn">The column to split the results on.</param>
    /// <param name="cancellationToken">A CancellationToken to cancel the asynchronous operation. Defaults to CancellationToken.None.</param>
    /// <returns>An IEnumerable containing the mapped results of the query.</returns>
    private async Task<IEnumerable<TReturn>> QueryInternalAsync<TFirst, TSecond, TReturn>(string sql, object filter,
        Func<TFirst, TSecond, TReturn> func, string splitOn, CancellationToken cancellationToken = default)
    {
        using var con = GetConnection();
        return await con
            .QueryAsync(new CommandDefinition(sql, filter, cancellationToken: cancellationToken), func, splitOn)
            .ConfigureAwait(false);
    }
    /// <summary>
    /// Executes a SQL query asynchronously and returns the results as an IEnumerable using a specified mapping function.
    /// </summary>
    /// <typeparam name="TFirst">The type of the first result set.</typeparam>
    /// <typeparam name="TSecond">The type of the second result set.</typeparam>
    /// <typeparam name="TThird">The type of the third result set.</typeparam>
    /// <typeparam name="TReturn">The type of the resulting objects after mapping.</typeparam>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="filter">The filter for the query. Defaults to null.</param>
    /// <param name="func">A mapping function for the query results.</param>
    /// <param name="splitOn">The column to split the results on.</param>
    /// <param name="cancellationToken">A CancellationToken to cancel the asynchronous operation. Defaults to CancellationToken.None.</param>
    /// <returns>An IEnumerable containing the mapped results of the query.</returns>
    private async Task<IEnumerable<TReturn>> QueryInternalAsync<TFirst, TSecond, TThird, TReturn>(string sql,
        object filter, Func<TFirst, TSecond, TThird, TReturn> func, string splitOn,
        CancellationToken cancellationToken = default)
    {
        using var con = GetConnection();
        return await con.QueryAsync(new CommandDefinition(sql, filter, cancellationToken: cancellationToken), func,
            splitOn).ConfigureAwait(false);
    }
    /// <summary>
    /// Executes a SQL query asynchronously and returns the results as an IEnumerable using a specified mapping function.
    /// </summary>
    /// <typeparam name="TFirst">The type of the first result set.</typeparam>
    /// <typeparam name="TSecond">The type of the second result set.</typeparam>
    /// <typeparam name="TThird">The type of the third result set.</typeparam>
    /// <typeparam name="TFourth">The type of the fourth result set.</typeparam>
    /// <typeparam name="TReturn">The type of the resulting objects after mapping.</typeparam>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="filter">The filter for the query. Defaults to null.</param>
    /// <param name="func">A mapping function for the query results.</param>
    /// <param name="splitOn">The column to split the results on.</param>
    /// <param name="cancellationToken">A CancellationToken to cancel the asynchronous operation. Defaults to CancellationToken.None.</param>
    /// <returns>An IEnumerable containing the mapped results of the query.</returns>
    private async Task<IEnumerable<TReturn>> QueryInternalAsync<TFirst, TSecond, TThird, TFourth, TReturn>(
        string sql, object filter, Func<TFirst, TSecond, TThird, TFourth, TReturn> func, string splitOn,
        CancellationToken cancellationToken = default)
    {
        using var con = GetConnection();
        return await con.QueryAsync(
                new CommandDefinition(sql, filter, cancellationToken: cancellationToken), func, splitOn)
            .ConfigureAwait(false);
    }
    /// <summary>
    /// Executes a SQL query asynchronously and returns the results as an IEnumerable using a specified mapping function.
    /// </summary>
    /// <typeparam name="TFirst">The type of the first result set.</typeparam>
    /// <typeparam name="TSecond">The type of the second result set.</typeparam>
    /// <typeparam name="TThird">The type of the third result set.</typeparam>
    /// <typeparam name="TFourth">The type of the fourth result set.</typeparam>
    /// <typeparam name="TFifth">The type of the fifth result set.</typeparam>
    /// <typeparam name="TReturn">The type of the resulting objects after mapping.</typeparam>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="filter">The filter for the query. Defaults to null.</param>
    /// <param name="func">A mapping function for the query results.</param>
    /// <param name="splitOn">The column to split the results on.</param>
    /// <param name="cancellationToken">A CancellationToken to cancel the asynchronous operation. Defaults to CancellationToken.None.</param>
    /// <returns>An IEnumerable containing the mapped results of the query.</returns>
    private async Task<IEnumerable<TReturn>> QueryInternalAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
        string sql, object filter, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> func, string splitOn,
        CancellationToken cancellationToken = default)
    {
        using var con = GetConnection();
        return await con.QueryAsync(
                new CommandDefinition(sql, filter, cancellationToken: cancellationToken), func, splitOn)
            .ConfigureAwait(false);
    }
    /// <summary>
    /// Executes a SQL query asynchronously and returns a scalar result of type T.
    /// </summary>
    /// <typeparam name="T">The type of the scalar result.</typeparam>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="filter">The filter for the query. Defaults to null.</param>
    /// <param name="dbTransaction">The IDbTransaction to use for the query. Defaults to null.</param>
    /// <param name="cancellationToken">A CancellationToken to cancel the asynchronous operation. Defaults to CancellationToken.None.</param>
    /// <returns>The scalar result of type T from the query.</returns>
    private async Task<T> ExecuteInternalScalar<T>(string sql, object filter = null,
        IDbTransaction dbTransaction = null, CancellationToken cancellationToken = default)
    {
        using var con = GetConnection();

        return await con
            .ExecuteScalarAsync<T>(new CommandDefinition(sql, filter, dbTransaction,
                cancellationToken: cancellationToken)).ConfigureAwait(false);
    }
    /// <summary>
    /// Executes a SQL query asynchronously and returns the number of affected rows.
    /// </summary>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="filter">The filter for the query. Defaults to null.</param>
    /// <param name="dbTransaction">The IDbTransaction to use for the query. Defaults to null.</param>
    /// <param name="cancellationToken">A CancellationToken to cancel the asynchronous operation. Defaults to CancellationToken.None.</param>
    /// <returns>The number of affected rows from the query.</returns>
    private async Task<int> ExecuteInternalAsync(string sql, object filter = null,
        IDbTransaction dbTransaction = null, CancellationToken cancellationToken = default)
    {
        using var con = GetConnection();

        return await con
            .ExecuteAsync(new CommandDefinition(sql, filter, dbTransaction, cancellationToken: cancellationToken))
            .ConfigureAwait(false);
    }
    /// <summary>
    /// Executes a paged SQL query asynchronously and returns the results as an IEnumerable.
    /// </summary>
    /// <typeparam name="T">The type of object to retrieve from the query.</typeparam>
    /// <param name="sql">The SQL query to execute.</param>
    /// <param name="filter">The filter for pagination and ordering.</param>
    /// <param name="cancellationToken">A CancellationToken to cancel the asynchronous operation. Defaults to CancellationToken.None.</param>
    /// <returns>An IEnumerable containing the results of the query.</returns>
    private async Task<IEnumerable<T>> QueryListPagedInternalAsync<T>(string sql, IPagedFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = sql.AddPagination(filter, dataSource);

        using var con = GetConnection();

        return await con.QueryAsync<T>(new CommandDefinition(query, filter, cancellationToken: cancellationToken))
            .ConfigureAwait(false);
    }
}