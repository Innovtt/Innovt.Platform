// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Dynamo

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.Table;
using Innovt.Core.Collections;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Polly.Retry;
using BatchGetItemRequest = Innovt.Cloud.Table.BatchGetItemRequest;
using BatchWriteItemRequest = Innovt.Cloud.Table.BatchWriteItemRequest;
using BatchWriteItemResponse = Innovt.Cloud.Table.BatchWriteItemResponse;
using QueryRequest = Innovt.Cloud.Table.QueryRequest;
using ScanRequest = Innovt.Cloud.Table.ScanRequest;

namespace Innovt.Cloud.AWS.Dynamo;

/// <summary>
///     Base repository class for interacting with AWS DynamoDB tables.
/// </summary>
public abstract class Repository : AwsBaseService, ITableRepository
{
    private static readonly ActivitySource ActivityRepository = new("Innovt.Cloud.AWS.Dynamo.Repository");

    private DynamoDBContext dynamoDbContext;
    private AmazonDynamoDBClient dynamoClient;
    private DynamoContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="configuration">AWS configuration.</param>
    protected Repository(ILogger logger, IAwsConfiguration configuration) : base(logger, configuration)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository" /> class using a context map.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    /// <param name="context"></param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Repository(ILogger logger, IAwsConfiguration configuration,DynamoContext context) : base(logger, configuration)
    {
        this.context = context ??throw new ArgumentNullException(nameof(context));
    }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="Repository" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="configuration">AWS configuration.</param>
    /// <param name="region">The AWS region.</param>
    protected Repository(ILogger logger, IAwsConfiguration configuration, string region) : base(logger,
        configuration, region)
    {
    }
    

    /// <summary>
    ///     Gets the DynamoDB context.
    /// </summary>
    private DynamoDBContext Context => dynamoDbContext ??= new DynamoDBContext(DynamoClient);

    /// <summary>
    ///     Gets the Amazon DynamoDB client.
    /// </summary>
    private AmazonDynamoDBClient DynamoClient => dynamoClient ??= CreateService<AmazonDynamoDBClient>();

    /// <summary>
    ///     Gets the default operation configuration for DynamoDB operations.
    /// </summary>
    private static DynamoDBOperationConfig OperationConfig =>
        new()
        {
            ConsistentRead = true,
            Conversion = DynamoDBEntryConversion.V2
        };

    /// <inheritdoc />
    public async Task<T> GetByIdAsync<T>(object id, string rangeKey = null,
        CancellationToken cancellationToken = default) where T : class, ITableMessage
    {
        using (ActivityRepository.StartActivity())
        {
            var policy = CreateDefaultRetryAsyncPolicy();

            if (string.IsNullOrEmpty(rangeKey))
                return await policy.ExecuteAsync(async () =>
                        await Context.LoadAsync<T>(id, OperationConfig, cancellationToken).ConfigureAwait(false))
                    .ConfigureAwait(false);

            return await policy
                .ExecuteAsync(async () =>
                    await Context.LoadAsync<T>(id, rangeKey, OperationConfig, cancellationToken)
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync<T>(T value, CancellationToken cancellationToken = default) where T : class, ITableMessage
    {
        using (ActivityRepository.StartActivity())
        {
            await CreateDefaultRetryAsyncPolicy()
                .ExecuteAsync(async () => await Context.DeleteAsync(value, cancellationToken).ConfigureAwait(false))
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync<T>(object id, string rangeKey = null, CancellationToken cancellationToken = default)
        where T : class, ITableMessage
    {
        if (id == null) throw new ArgumentNullException(nameof(id));

        using var activity = ActivityRepository.StartActivity();
        activity?.SetTag("id", id);

        var policy = CreateDefaultRetryAsyncPolicy();

        if (string.IsNullOrEmpty(rangeKey))
            await policy.ExecuteAsync(async () =>
                    await Context.DeleteAsync<T>(id, cancellationToken).ConfigureAwait(false))
                .ConfigureAwait(false);
        else
            await policy.ExecuteAsync(async () =>
                    await Context.DeleteAsync<T>(id, rangeKey, cancellationToken).ConfigureAwait(false))
                .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task AddAsync<T>(T message, CancellationToken cancellationToken = default) where T : class, ITableMessage
    {
        using (ActivityRepository.StartActivity(nameof(DeleteAsync)))
        {
            await CreateDefaultRetryAsyncPolicy()
                .ExecuteAsync(async () =>
                    await Context.SaveAsync(message, OperationConfig, cancellationToken).ConfigureAwait(false))
                .ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     Adds a list of items to the DynamoDB table.
    /// </summary>
    /// <typeparam name="T">The type of items to add.</typeparam>
    /// <param name="messages">The list of items to add.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <exception cref="ArgumentNullException">Thrown if the messages parameter is null.</exception>
    public async Task AddAsync<T>(IList<T> messages, CancellationToken cancellationToken = default)
        where T : class, ITableMessage
    {
        if (messages is null) throw new ArgumentNullException(nameof(messages));

        using (ActivityRepository.StartActivity())
        {
            var batch = Context.CreateBatchWrite<T>(OperationConfig);

            batch.AddPutItems(messages);

            await CreateDefaultRetryAsyncPolicy()
                .ExecuteAsync(async () => await batch.ExecuteAsync(cancellationToken).ConfigureAwait(false))
                .ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     When you don't know the type of the object to add or if you want to ad a list of different types
    /// </summary>
    /// <param name="messages"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task AddAsync(IList<object> messages, CancellationToken cancellationToken = default)
    {
        if (messages is null) throw new ArgumentNullException(nameof(messages));

        using (ActivityRepository.StartActivity())
        {
            var batch = Context.CreateBatchWrite<object>(OperationConfig);

            batch.AddPutItems(messages);

            await CreateDefaultRetryAsyncPolicy()
                .ExecuteAsync(async () => await batch.ExecuteAsync(cancellationToken).ConfigureAwait(false))
                .ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     Queries the DynamoDB table and returns the first item with the specified id.
    /// </summary>
    /// <typeparam name="T">The type of item to query.</typeparam>
    /// <param name="id">The id to query.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>The first item found with the specified id.</returns>
    public async Task<T> QueryFirstAsync<T>(object id, CancellationToken cancellationToken = default)
    {
        using (ActivityRepository.StartActivity())
        {
            var result = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await Context.QueryAsync<T>(id, OperationConfig).GetNextSetAsync(cancellationToken)
                        .ConfigureAwait(false))
                .ConfigureAwait(false);

            return result == null ? default : result.FirstOrDefault();
        }
    }

    /// <summary>
    ///     Queries the DynamoDB table and returns a list of items with the specified id.
    /// </summary>
    /// <typeparam name="T">The type of items to query.</typeparam>
    /// <param name="id">The id to query.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A list of items with the specified id.</returns>
    public async Task<IList<T>> QueryAsync<T>(object id, CancellationToken cancellationToken = default) where T : class
    {
        using (ActivityRepository.StartActivity())
        {
            var result = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await Context.QueryAsync<T>(id, OperationConfig).GetRemainingAsync(cancellationToken)
                        .ConfigureAwait(false))
                .ConfigureAwait(false);

            return result;
        }
    }

    /// <summary>
    ///     Queries the DynamoDB table using a query request and returns a list of items.
    /// </summary>
    /// <typeparam name="T">The type of items to query.</typeparam>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A list of items based on the query request.</returns>
    public async Task<IList<T>> QueryAsync<T>(QueryRequest request, CancellationToken cancellationToken = default) where T : class
    {
        using (ActivityRepository.StartActivity())
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            //var (_, items) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

            var values = new Dictionary<string, AttributeValue>();
            
            values.Add("Name2", new AttributeValue("Michel"));
            values.Add("JobPositionId", new AttributeValue("1"));
            values.Add("Email", new AttributeValue("michelmob@gmail.com"));
            values.Add("EmailToBeIgnored", new AttributeValue("michelmob2@gmail.com"));
            values.Add("Id", new AttributeValue(){N = "1"});
            values.Add("CorrelationId", new AttributeValue(Guid.NewGuid().ToString()));
            values.Add("CreatedAt", new AttributeValue(DateTime.Now.ToString(CultureInfo.InvariantCulture)));
            
            var items = new List<Dictionary<string, AttributeValue>>()
            {
                values
            };
            
            return Helpers.ConvertAttributesToType<T>(items,context);
        }
    }

    /// <summary>
    ///     Queries the DynamoDB table using a query request and splits the results into two types based on a key.
    /// </summary>
    /// <typeparam name="T">The type of items to query.</typeparam>
    /// <typeparam name="TResult1">The first type to split the results into.</typeparam>
    /// <typeparam name="TResult2">The second type to split the results into.</typeparam>
    /// <param name="request">The query request.</param>
    /// <param name="splitBy">The key to split the results.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>Tuple containing lists of items split based on the specified key.</returns>
    public async Task<(IList<TResult1> first, IList<TResult2> second)> QueryMultipleAsync<T, TResult1, TResult2>(
        QueryRequest request, string splitBy, CancellationToken cancellationToken = default) where T : class
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        using (ActivityRepository.StartActivity())
        {
            var (_, items) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

            return Helpers.ConvertAttributesToType<TResult1, TResult2>(items, splitBy);
        }
    }

    /// <summary>
    ///     Queries the DynamoDB table using a query request and splits the results into three types based on specified keys.
    /// </summary>
    /// <typeparam name="T">The type of items to query.</typeparam>
    /// <typeparam name="TResult1">The first type to split the results into.</typeparam>
    /// <typeparam name="TResult2">The second type to split the results into.</typeparam>
    /// <typeparam name="TResult3">The third type to split the results into.</typeparam>
    /// <param name="request">The query request.</param>
    /// <param name="splitBy">The keys to split the results.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>Tuple containing lists of items split based on the specified keys.</returns>
    public async Task<(IList<TResult1> first, IList<TResult2> second, IList<TResult3> third)>
        QueryMultipleAsync<T, TResult1, TResult2, TResult3>(QueryRequest request, string[] splitBy,
            CancellationToken cancellationToken = default) where T : class
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        if (splitBy == null) throw new ArgumentNullException(nameof(splitBy));

        using (ActivityRepository.StartActivity())
        {
            var (_, items) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

            return Helpers.ConvertAttributesToType<TResult1, TResult2, TResult3>(items, splitBy);
        }
    }

    /// <summary>
    ///     Queries the DynamoDB table using a query request and splits the results into four types based on specified keys.
    /// </summary>
    /// <typeparam name="T">The type of items to query.</typeparam>
    /// <typeparam name="TResult1">The first type to split the results into.</typeparam>
    /// <typeparam name="TResult2">The second type to split the results into.</typeparam>
    /// <typeparam name="TResult3">The third type to split the results into.</typeparam>
    /// <typeparam name="TResult4">The fourth type to split the results into.</typeparam>
    /// <param name="request">The query request.</param>
    /// <param name="splitBy">The keys to split the results.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>Tuple containing lists of items split based on the specified keys.</returns>
    public async Task<(IList<TResult1> first, IList<TResult2> second, IList<TResult3> third, IList<TResult4> fourth)>
        QueryMultipleAsync<T, TResult1, TResult2, TResult3, TResult4>(QueryRequest request, string[] splitBy,
            CancellationToken cancellationToken = default) where T : class
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        if (splitBy == null) throw new ArgumentNullException(nameof(splitBy));

        using (ActivityRepository.StartActivity())
        {
            var (_, items) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

            return Helpers.ConvertAttributesToType<TResult1, TResult2, TResult3, TResult4>(items, splitBy);
        }
    }

    /// <summary>
    ///     Queries the DynamoDB table using a query request and splits the results into five types based on specified keys.
    /// </summary>
    /// <typeparam name="T">The type of items to query.</typeparam>
    /// <typeparam name="TResult1">The first type to split the results into.</typeparam>
    /// <typeparam name="TResult2">The second type to split the results into.</typeparam>
    /// <typeparam name="TResult3">The third type to split the results into.</typeparam>
    /// <typeparam name="TResult4">The fourth type to split the results into.</typeparam>
    /// <typeparam name="TResult5">The fifth type to split the results into.</typeparam>
    /// <param name="request">The query request.</param>
    /// <param name="splitBy">The keys to split the results.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>Tuple containing lists of items split based on the specified keys.</returns>
    public async Task<(IList<TResult1> first, IList<TResult2> second, IList<TResult3> third, IList<TResult4> fourth,
            IList<TResult5> fifth)>
        QueryMultipleAsync<T, TResult1, TResult2, TResult3, TResult4, TResult5>(QueryRequest request, string[] splitBy,
            CancellationToken cancellationToken = default) where T : class
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        if (splitBy == null) throw new ArgumentNullException(nameof(splitBy));

        using (ActivityRepository.StartActivity())
        {
            var (_, items) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

            return Helpers.ConvertAttributesToType<TResult1, TResult2, TResult3, TResult4, TResult5>(items, splitBy);
        }
    }

    /// <summary>
    ///     Queries the DynamoDB table and returns the first or default item based on the query request.
    /// </summary>
    /// <typeparam name="T">The type of item to query.</typeparam>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>The first or default item based on the query request.</returns>
    public async Task<T> QueryFirstOrDefaultAsync<T>(QueryRequest request,
        CancellationToken cancellationToken = default) where T : class
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        using (ActivityRepository.StartActivity())
        {
            request.PageSize = 1;
            request.Page = null;

            var (_, items) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

            var queryResponse = Helpers.ConvertAttributesToType<T>(items);

            return queryResponse.FirstOrDefault();
        }
    }

    /// <summary>
    ///     Queries the DynamoDB table using a query request and returns a paginated collection of items.
    /// </summary>
    /// <typeparam name="T">The type of items to query.</typeparam>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A paginated collection of items based on the query request.</returns>
    public async Task<PagedCollection<T>> QueryPaginatedByAsync<T>(QueryRequest request,
        CancellationToken cancellationToken = default) where T : class
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        using (ActivityRepository.StartActivity(nameof(QueryFirstOrDefaultAsync)))
        {
            var (lastEvaluatedKey, items) =
                await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

            return new PagedCollection<T>
            {
                Items = Helpers.ConvertAttributesToType<T>(items),
                Page = Helpers.CreatePaginationToken(lastEvaluatedKey),
                PageSize = request.PageSize.GetValueOrDefault()
            };
        }
    }

    /// <summary>
    ///     Writes a batch of transactional write items to the DynamoDB table.
    /// </summary>
    /// <param name="request">The transaction write request.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    /// <exception cref="BusinessException">Thrown when the number of transactItems is invalid.</exception>
    public async Task TransactWriteItemsAsync(TransactionWriteRequest request, CancellationToken cancellationToken)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        if (request.TransactItems is null || request.TransactItems.Count is > 25 or 0)
            throw new BusinessException(
                "The number of transactItems should be greater than 0 and less or equal than 25");

        using var activity = ActivityRepository.StartActivity();
        activity?.SetTag("TransactWriteItems", request.TransactItems.Count);

        var transactRequest = new TransactWriteItemsRequest
        {
            ClientRequestToken = request.ClientRequestToken,
            TransactItems = new List<TransactWriteItem>()
        };

        foreach (var transactItem in request.TransactItems)
            transactRequest.TransactItems.Add(Helpers.CreateTransactionWriteItem(transactItem));


        await DynamoClient.TransactWriteItemsAsync(transactRequest, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///     Scans the DynamoDB table based on the specified scan request.
    /// </summary>
    /// <typeparam name="T">The type of items to scan.</typeparam>
    /// <param name="request">The scan request.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>The list of scanned items.</returns>
    public async Task<IList<T>> ScanAsync<T>(ScanRequest request,
        CancellationToken cancellationToken = default) where T : class
    {
        using (ActivityRepository.StartActivity())
        {
            return (await InternalScanAsync<T>(request, cancellationToken).ConfigureAwait(false)).Items;
        }
    }

    /// <summary>
    ///     Scans the DynamoDB table based on the specified scan request and returns a paginated collection of items.
    /// </summary>
    /// <typeparam name="T">The type of items to scan.</typeparam>
    /// <param name="request">The scan request.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A paginated collection of items based on the scan request.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    public async Task<PagedCollection<T>> ScanPaginatedByAsync<T>(ScanRequest request,
        CancellationToken cancellationToken = default) where T : class
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        using (ActivityRepository.StartActivity())
        {
            var (exclusiveStartKey, items) =
                await InternalScanAsync<T>(request, cancellationToken).ConfigureAwait(false);

            if (items?.Count == 0)
                return new PagedCollection<T>();

            var response = new PagedCollection<T>
            {
                Items = items,
                Page = Helpers.CreatePaginationToken(exclusiveStartKey),
                PageSize = request.PageSize.GetValueOrDefault()
            };

            return response;
        }
    }

    /// <summary>
    ///     Executes a SQL statement asynchronously and returns the response containing items of type T.
    /// </summary>
    /// <typeparam name="T">The type of items to be returned.</typeparam>
    /// <param name="sqlStatementRequest">The SQL statement request.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>The response containing items of type T.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the SQL statement request is null.</exception>
    public async Task<ExecuteSqlStatementResponse<T>> ExecuteStatementAsync<T>(
        ExecuteSqlStatementRequest sqlStatementRequest, CancellationToken cancellationToken = default) where T : class
    {
        if (sqlStatementRequest is null) throw new ArgumentNullException(nameof(sqlStatementRequest));

        using (ActivityRepository.StartActivity(nameof(UpdateAsync)))
        {
            var response = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await DynamoClient.ExecuteStatementAsync(new ExecuteStatementRequest
                {
                    ConsistentRead = sqlStatementRequest.ConsistentRead,
                    NextToken = sqlStatementRequest.NextToken,
                    Statement = sqlStatementRequest.Statment
                }, cancellationToken).ConfigureAwait(false)).ConfigureAwait(false);


            if (response is null)
                return null;

            return new ExecuteSqlStatementResponse<T>
            {
                NextToken = sqlStatementRequest.NextToken,
                Items = Helpers.ConvertAttributesToType<T>(response.Items)
            };
        }
    }

    /// <summary>
    ///     Gets a batch of items from the DynamoDB table based on the specified batch get item request.
    /// </summary>
    /// <typeparam name="T">The type of items to get.</typeparam>
    /// <param name="batchGetItemRequest">The batch get item request.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>The list of items retrieved.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the batch get item request is null.</exception>
    public async Task<List<T>> BatchGetItem<T>(BatchGetItemRequest batchGetItemRequest,
        CancellationToken cancellationToken = default) where T : class
    {
        if (batchGetItemRequest is null) throw new ArgumentNullException(nameof(batchGetItemRequest));

        using (ActivityRepository.StartActivity())
        {
            var items = Helpers.CreateBatchGetItemRequest(batchGetItemRequest);

            var response = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await DynamoClient.BatchGetItemAsync(items, cancellationToken).ConfigureAwait(false))
                .ConfigureAwait(false);

            if (response.Responses is null)
                return null;

            var result = new List<T>();

            foreach (var item in response.Responses) result.AddRange(Helpers.ConvertAttributesToType<T>(item.Value));

            return result;
        }
    }

    /// <summary>
    ///     Writes a batch of items to the DynamoDB table based on the specified batch write item request.
    /// </summary>
    /// <param name="batchWriteItemRequest">The batch write item request.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>The response indicating the result of the batch write operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the batch write item request is null.</exception>
    public async Task<BatchWriteItemResponse> BatchWriteItem(BatchWriteItemRequest batchWriteItemRequest,
        CancellationToken cancellationToken = default)
    {
        if (batchWriteItemRequest is null) throw new ArgumentNullException(nameof(batchWriteItemRequest));

        using (ActivityRepository.StartActivity())
        {
            var request = Helpers.CreateBatchWriteItemRequest(batchWriteItemRequest);
            var result = new BatchWriteItemResponse();
            var attempts = 0;
            var backOfficeRandom = new Random(1);

            do
            {
                var response = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                        await DynamoClient.BatchWriteItemAsync(request, cancellationToken).ConfigureAwait(false))
                    .ConfigureAwait(false);

                if (response.UnprocessedItems.IsNullOrEmpty())
                    return result;

                request.RequestItems = response.UnprocessedItems;
                attempts++;

                Thread.Sleep(
                    TimeSpan.FromSeconds(batchWriteItemRequest.RetryDelay.Seconds * backOfficeRandom.Next(1, 3)));
            } while (attempts < batchWriteItemRequest.MaxRetry);

            return result;
        }
    }

    /// <summary>
    ///     Creates the default asynchronous retry policy for handling exceptions during operations.
    /// </summary>
    /// <returns>The asynchronous retry policy instance.</returns>
    protected override AsyncRetryPolicy CreateDefaultRetryAsyncPolicy()
    {
        return base.CreateRetryAsyncPolicy<ProvisionedThroughputExceededException,
            InternalServerErrorException, LimitExceededException, ResourceInUseException>();
    }

    /// <summary>
    ///     Updates an item in the DynamoDB table based on the provided parameters.
    /// </summary>
    /// <param name="tableName">The name of the DynamoDB table.</param>
    /// <param name="key">The key of the item to update.</param>
    /// <param name="attributeUpdates">The attribute updates for the item.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>Task representing the asynchronous update operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key or attributeUpdates is null.</exception>
    protected async Task UpdateAsync(string tableName, Dictionary<string, AttributeValue> key,
        Dictionary<string, AttributeValueUpdate> attributeUpdates, CancellationToken cancellationToken = default)
    {
        if (key is null) throw new ArgumentNullException(nameof(key));

        if (attributeUpdates is null) throw new ArgumentNullException(nameof(attributeUpdates));

        using (ActivityRepository.StartActivity())
        {
            await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await DynamoClient.UpdateItemAsync(tableName, key, attributeUpdates, cancellationToken)
                        .ConfigureAwait(false))
                .ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     Updates an item in the DynamoDB table based on the provided update item request.
    /// </summary>
    /// <typeparam name="T">The type of item to update.</typeparam>
    /// <param name="updateItemRequest">The update item request.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>The updated item of type T.</returns>
    /// <exception cref="ArgumentNullException">Thrown when updateItemRequest is null.</exception>
    protected async Task<T> UpdateAsync<T>(UpdateItemRequest updateItemRequest,
        CancellationToken cancellationToken = default)
    {
        if (updateItemRequest is null)
            throw new ArgumentNullException(nameof(updateItemRequest));

        using var activity = ActivityRepository.StartActivity();
        var response = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await DynamoClient.UpdateItemAsync(updateItemRequest,
                    cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);

        return response.Attributes.IsNullOrEmpty()
            ? default
            : AttributeConverter.ConvertAttributesToType<T>(response.Attributes);
    }

    /// <summary>
    ///     Executes an internal query against the DynamoDB table based on the specified query request.
    /// </summary>
    /// <typeparam name="T">The type of items to query.</typeparam>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A tuple containing the last evaluated key and the list of items retrieved.</returns>
    /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
    private async
        Task<(Dictionary<string, AttributeValue> LastEvaluatedKey, IList<Dictionary<string, AttributeValue>> Items)>
        InternalQueryAsync<T>(QueryRequest request, CancellationToken cancellationToken = default) where T : class
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        using (ActivityRepository.StartActivity())
        {
            var queryRequest = Helpers.CreateQueryRequest<T>(request,context);

            var items = new List<Dictionary<string, AttributeValue>>();
            var remaining = request.PageSize;

            var iterator = DynamoClient.Paginators.Query(queryRequest).Responses
                .GetAsyncEnumerator(cancellationToken);

            Dictionary<string, AttributeValue> lastEvaluatedKey = null;

            do
            {
                await iterator.MoveNextAsync().ConfigureAwait(false);

                if (iterator.Current == null)
                    break;

                items.AddRange(iterator.Current.Items);
                queryRequest.ExclusiveStartKey = lastEvaluatedKey = iterator.Current.LastEvaluatedKey;

                remaining = remaining.HasValue ? request.PageSize - items.Count : 0;

                if (remaining > 0) queryRequest.Limit = remaining.Value;
            } while (lastEvaluatedKey.Count > 0 && remaining > 0);

            return (lastEvaluatedKey, items);
        }
    }

    /// <summary>
    ///     Executes an internal scan against the DynamoDB table based on the specified scan request.
    /// </summary>
    /// <typeparam name="T">The type of items to scan.</typeparam>
    /// <param name="request">The scan request.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A tuple containing the last evaluated key and the list of items retrieved.</returns>
    /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
    private async Task<(Dictionary<string, AttributeValue> ExclusiveStartKey, IList<T> Items)> InternalScanAsync<T>(
        ScanRequest request, CancellationToken cancellationToken = default) where T : class
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        using (ActivityRepository.StartActivity())
        {
            var scanRequest = Helpers.CreateScanRequest<T>(request);

            Dictionary<string, AttributeValue> lastEvaluatedKey = null;

            var items = new List<T>();
            var remaining = request.PageSize;

            var iterator = DynamoClient.Paginators.Scan(scanRequest).Responses.GetAsyncEnumerator(cancellationToken);

            //TODO: Thi code is the same in InternalQuery - Refactory it
            do
            {
                await iterator.MoveNextAsync().ConfigureAwait(false);

                if (iterator.Current == null)
                    break;

                items.AddRange(Helpers.ConvertAttributesToType<T>(iterator.Current.Items));
                scanRequest.ExclusiveStartKey = lastEvaluatedKey = iterator.Current.LastEvaluatedKey;
                remaining = remaining.HasValue ? request.PageSize - items.Count : 0;

                if (remaining > 0) scanRequest.Limit = remaining.Value;
            } while (lastEvaluatedKey.Count > 0 && remaining > 0);

            return (lastEvaluatedKey, items);
        }
    }

    /// <summary>
    ///     Disposes the DynamoDB context and AmazonDynamoDBClient resources.
    /// </summary>
    protected override void DisposeServices()
    {
        dynamoDbContext?.Dispose();
        dynamoClient?.Dispose();
    }
}