// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Collections;

namespace Innovt.Cloud.Table;

/// <summary>
///     Interface representing a repository for interacting with a table.
/// </summary>
public interface ITableRepository : IDisposable
{
    /// <summary>
    ///     Asynchronously adds a single item to the repository.
    /// </summary>
    /// <typeparam name="T">The type of item to add.</typeparam>
    /// <param name="message">The item to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous add operation.</returns>
    Task AddAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    ///     Asynchronously adds multiple items to the repository.
    /// </summary>
    /// <typeparam name="T">The type of items to add.</typeparam>
    /// <param name="messages">The list of items to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous add operation.</returns>
    Task AddRangeAsync<T>(ICollection<T> messages, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    ///     This method will perform an update operation on the table. The operation is based on the primary key and type is
    ///     PUT.
    /// </summary>
    /// <param name="instance">The instance that you want to update</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <typeparam name="T">The instance updated.</typeparam>
    /// <returns></returns>
    Task<T> UpdateAsync<T>(T instance, CancellationToken cancellationToken = default) where T : class, new();

    /// <summary>
    ///     Asynchronously performs a transactional write of items.
    /// </summary>
    /// <param name="request">The transaction write request specifying the items to write.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous transactional write operation.</returns>
    Task TransactWriteItemsAsync(TransactionWriteRequest request, CancellationToken cancellationToken);

    /// <summary>
    ///     Asynchronously performs a batch write operation.
    /// </summary>
    /// <param name="batchWriteItemRequest">The batch write item request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A response from the batch write operation.</returns>
    Task<BatchWriteItemResponse> BatchWriteItem(BatchWriteItemRequest batchWriteItemRequest,
        CancellationToken cancellationToken = default);


    /// <summary>
    ///     Asynchronously deletes an item using its value.
    /// </summary>
    /// <typeparam name="T">The type of item to delete.</typeparam>
    /// <param name="message">The item to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    Task DeleteAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    ///     Asynchronously deletes an item using its identifier and optional range key.
    /// </summary>
    /// <typeparam name="T">The type of item to delete.</typeparam>
    /// <param name="id">The identifier of the item.</param>
    /// <param name="rangeKey">The range key for the item (optional).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    Task DeleteAsync<T>(object id, string rangeKey = null, CancellationToken cancellationToken = default)
        where T : class;

    /// <summary>
    ///     Asynchronously a list of item that are from the same type.
    /// </summary>
    /// <param name="messages"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task DeleteRangeAsync<T>(ICollection<T> messages, CancellationToken cancellationToken = default) where T : class;


    /// <summary>
    ///     Asynchronously retrieves an item by its identifier.
    /// </summary>
    /// <typeparam name="T">The type of item to retrieve.</typeparam>
    /// <param name="id">The identifier of the item.</param>
    /// <param name="rangeKey">The range key for the item (optional).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An item of type T if found; otherwise, null.</returns>
    Task<T> GetByIdAsync<T>(object id, string rangeKey = null, CancellationToken cancellationToken = default)
        where T : class;

    /// <summary>
    ///     Asynchronously queries and retrieves the first item of type T by its identifier.
    /// </summary>
    /// <typeparam name="T">The type of item to retrieve.</typeparam>
    /// <param name="id">The identifier of the item.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An item of type T if found; otherwise, null.</returns>
    Task<T> QueryFirstAsync<T>(object id, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    ///     Asynchronously queries and retrieves a list of items of type T by their identifier.
    /// </summary>
    /// <typeparam name="T">The type of items to retrieve.</typeparam>
    /// <param name="id">The identifier of the items.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of items of type T.</returns>
    Task<IList<T>> QueryAsync<T>(object id, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    ///     Asynchronously queries and retrieves a list of items of type T based on the provided query request.
    /// </summary>
    /// <typeparam name="T">The type of items to retrieve.</typeparam>
    /// <param name="request">The query request specifying the query parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of items of type T based on the query request.</returns>
    Task<IList<T>> QueryAsync<T>(QueryRequest request,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    ///     Asynchronously queries and retrieves the first item of type T based on the provided query request.
    /// </summary>
    /// <typeparam name="T">The type of item to retrieve.</typeparam>
    /// <param name="request">The query request specifying the query parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The first item of type T based on the query request; otherwise, null.</returns>
    Task<T> QueryFirstOrDefaultAsync<T>(QueryRequest request, CancellationToken cancellationToken = default)
        where T : class;

    /// <summary>
    ///     Asynchronously queries and retrieves multiple sets of results from a single query.
    /// </summary>
    /// <typeparam name="T">The type of items to retrieve.</typeparam>
    /// <typeparam name="TResult1">The type of the first result set.</typeparam>
    /// <typeparam name="TResult2">The type of the second result set.</typeparam>
    /// <param name="request">The query request.</param>
    /// <param name="splitBy">The parameter to split the results by.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Tuple containing the first and second result sets.</returns>
    Task<(IList<TResult1> first, IList<TResult2> second)> QueryMultipleAsync<T, TResult1, TResult2>(
        QueryRequest request, string splitBy, CancellationToken cancellationToken = default) where T : class
        where TResult1 : class
        where TResult2 : class;

    /// <summary>
    ///     Asynchronously queries and retrieves multiple sets of results from a single query.
    /// </summary>
    /// <typeparam name="T">The type of items to retrieve.</typeparam>
    /// <typeparam name="TResult1">The type of the first result set.</typeparam>
    /// <typeparam name="TResult2">The type of the second result set.</typeparam>
    /// <typeparam name="TResult3">The type of the third result set.</typeparam>
    /// <param name="request">The query request specifying the query parameters.</param>
    /// <param name="splitBy">The parameter to split the results by.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Tuple containing the first, second, and third result sets.</returns>
    Task<(IList<TResult1> first, IList<TResult2> second, IList<TResult3> third)> QueryMultipleAsync<T, TResult1,
        TResult2, TResult3>(QueryRequest request, string[] splitBy,
        CancellationToken cancellationToken = default) where T : class
        where TResult1 : class
        where TResult2 : class
        where TResult3 : class;

    /// <summary>
    ///     Asynchronously queries and retrieves multiple sets of results from a single query.
    /// </summary>
    /// <typeparam name="T">The type of items to retrieve.</typeparam>
    /// <typeparam name="TResult1">The type of the first result set.</typeparam>
    /// <typeparam name="TResult2">The type of the second result set.</typeparam>
    /// <typeparam name="TResult3">The type of the third result set.</typeparam>
    /// <typeparam name="TResult4">The type of the fourth result set.</typeparam>
    /// <param name="request">The query request specifying the query parameters.</param>
    /// <param name="splitBy">The parameter to split the results by.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Tuple containing the first, second, third, and fourth result sets.</returns>
    Task<(IList<TResult1> first, IList<TResult2> second, IList<TResult3> third, IList<TResult4> fourth)>
        QueryMultipleAsync<T, TResult1, TResult2, TResult3, TResult4>(QueryRequest request, string[] splitBy,
            CancellationToken cancellationToken = default) where T : class
        where TResult1 : class
        where TResult2 : class
        where TResult3 : class
        where TResult4 : class;

    /// <summary>
    ///     Asynchronously queries and retrieves multiple sets of results from a single query.
    /// </summary>
    /// <typeparam name="T">The type of items to retrieve.</typeparam>
    /// <typeparam name="TResult1">The type of the first result set.</typeparam>
    /// <typeparam name="TResult2">The type of the second result set.</typeparam>
    /// <typeparam name="TResult3">The type of the third result set.</typeparam>
    /// <typeparam name="TResult4">The type of the fourth result set.</typeparam>
    /// <typeparam name="TResult5">The type of the fifth result set.</typeparam>
    /// <param name="request">The query request specifying the query parameters.</param>
    /// <param name="splitBy">The parameter to split the results by.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Tuple containing the first, second, third, fourth, and fifth result sets.</returns>
    Task<(IList<TResult1> first, IList<TResult2> second, IList<TResult3> third, IList<TResult4> fourth, IList<TResult5>
            fifth)>
        QueryMultipleAsync<T, TResult1, TResult2, TResult3, TResult4, TResult5>(QueryRequest request, string[] splitBy,
            CancellationToken cancellationToken = default) where T : class
        where TResult1 : class
        where TResult2 : class
        where TResult3 : class
        where TResult4 : class
        where TResult5 : class;


    /// <summary>
    ///     Asynchronously scans and retrieves a list of items of type T based on the provided scan request.
    /// </summary>
    /// <typeparam name="T">The type of items to retrieve.</typeparam>
    /// <param name="request">The scan request specifying the scan parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of items of type T based on the scan request.</returns>
    Task<IList<T>> ScanAsync<T>(ScanRequest request,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    ///     Asynchronously scans and retrieves a paged collection of items of type T based on the provided scan request.
    /// </summary>
    /// <typeparam name="T">The type of items to retrieve.</typeparam>
    /// <param name="request">The scan request specifying the scan parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paged collection of items of type T based on the scan request.</returns>
    Task<PagedCollection<T>>
        ScanPaginatedByAsync<T>(ScanRequest request, CancellationToken cancellationToken = default)
        where T : class;

    /// <summary>
    ///     Asynchronously queries and retrieves a paged collection of items of type T based on the provided query request.
    /// </summary>
    /// <typeparam name="T">The type of items to retrieve.</typeparam>
    /// <param name="request">The query request specifying the query parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paged collection of items of type T based on the query request.</returns>
    Task<PagedCollection<T>> QueryPaginatedByAsync<T>(QueryRequest request,
        CancellationToken cancellationToken = default) where T : class;


    /// <summary>
    ///     Executes an SQL statement asynchronously and retrieves the results.
    /// </summary>
    /// <typeparam name="T">The type of results to expect.</typeparam>
    /// <param name="sqlStatementRequest">The SQL statement request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A response containing the results of the SQL statement execution.</returns>
    Task<ExecuteSqlStatementResponse<T>> ExecuteStatementAsync<T>(ExecuteSqlStatementRequest sqlStatementRequest,
        CancellationToken cancellationToken = default) where T : class, new();

    /// <summary>
    ///     Asynchronously retrieves multiple items in a batch.
    /// </summary>
    /// <typeparam name="T">The type of items to retrieve.</typeparam>
    /// <param name="batchGetItemRequest">The batch get item request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of items of type T.</returns>
    Task<List<T>> BatchGetItem<T>(BatchGetItemRequest batchGetItemRequest,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    ///     Create a transaction write item based on the instance.
    /// </summary>
    /// <param name="instance">A mapped instance with context.</param>
    /// <param name="operationType">The operation that you want to perform.</param>
    /// <typeparam name="T">A typed mapped entity</typeparam>
    /// <returns>A incomplete transaction write item with properties mapped.</returns>
    TransactionWriteItem CreateTransactionWriteItem<T>(T instance,
        TransactionWriteOperationType operationType = TransactionWriteOperationType.Put) where T : class, new();
}