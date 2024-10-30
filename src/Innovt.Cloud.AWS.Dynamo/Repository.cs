// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Dynamo

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.AWS.Dynamo.Converters;
using Innovt.Cloud.AWS.Dynamo.Converters.Attributes;
using Innovt.Cloud.AWS.Dynamo.Helpers;
using Innovt.Cloud.Table;
using Innovt.Core.Collections;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Innovt.Core.Utilities;
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
    //log constants 
    private const string RequestId = "RequestId";
    private const string ConsumedCapacity = "ConsumedCapacity";
    private const string StatusCode = "StatusCode";

    private static readonly ActivitySource ActivityRepository = new("Innovt.Cloud.AWS.Dynamo.Repository");
    private readonly DynamoContext context;

    private AmazonDynamoDBClient dynamoClient;

    #region [Configuration]

    /// <summary>
    ///     Gets the Amazon DynamoDB client.
    /// </summary>
    private AmazonDynamoDBClient DynamoClient => dynamoClient ??= CreateService<AmazonDynamoDBClient>();

    #endregion

    /// <summary>
    ///     Executes a SQL statement asynchronously and returns the response containing items of type T.
    /// </summary>
    /// <typeparam name="T">The type of items to be returned.</typeparam>
    /// <param name="sqlStatementRequest">The SQL statement request.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>The response containing items of type T.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the SQL statement request is null.</exception>
    public async Task<ExecuteSqlStatementResponse<T>> ExecuteStatementAsync<T>(
        ExecuteSqlStatementRequest sqlStatementRequest, CancellationToken cancellationToken = default)
        where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(sqlStatementRequest);

        using (ActivityRepository.StartActivity())
        {
            var response = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(ct =>
                DynamoClient.ExecuteStatementAsync(new ExecuteStatementRequest
                {
                    ConsistentRead = sqlStatementRequest.ConsistentRead,
                    NextToken = sqlStatementRequest.NextToken,
                    Statement = sqlStatementRequest.Statment
                }, ct), cancellationToken).ConfigureAwait(false);


            if (response is null)
                return null;

            return new ExecuteSqlStatementResponse<T>
            {
                NextToken = sqlStatementRequest.NextToken,
                Items = QueryHelper.ConvertAttributesToType<T>(response.Items, context)
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
        ArgumentNullException.ThrowIfNull(batchGetItemRequest);

        using (ActivityRepository.StartActivity())
        {
            var items = QueryHelper.CreateBatchGetItemRequest(batchGetItemRequest);

            var response = await CreateDefaultRetryAsyncPolicy()
                .ExecuteAsync(ct => DynamoClient.BatchGetItemAsync(items, ct), cancellationToken)
                .ConfigureAwait(false);

            if (response.Responses is null)
                return null;

            var result = new List<T>();

            foreach (var item in response.Responses)
                result.AddRange(QueryHelper.ConvertAttributesToType<T>(item.Value, context));

            return result;
        }
    }

    public TransactionWriteItem CreateTransactionWriteItem<T>(T instance,
        TransactionWriteOperationType operationType = TransactionWriteOperationType.Put) where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(instance);

        var attributes = new Dictionary<string, AttributeValue>();

        //Delete operation does not need attributes
        if (operationType != TransactionWriteOperationType.Delete)
            attributes = AttributeConverter.ConvertToAttributeValueMap(instance, context);

        var keys = TableHelper.ExtractKeyAttributeValueMap(instance, context);

        var transaction = new TransactionWriteItem
        {
            TableName = TableHelper.GetTableName<T>(context),
            OperationType = operationType,
            Items = attributes.ToDictionary(x => x.Key, object (x) => x.Value),
            Keys = keys.ToDictionary(x =>
                x.Key, object (x) => x.Value.S)
        };

        return transaction;
    }

    /// <summary>
    ///     Execute a batch write item request returning the unprocessed items after some retries
    /// </summary>
    /// <param name="batchWriteItemRequest"></param>
    /// <param name="maxRetry"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, List<WriteRequest>>> InternalBatchWriteItemAsync(
        Dictionary<string, List<WriteRequest>> batchWriteItemRequest,
        int maxRetry = 3, CancellationToken cancellationToken = default)
    {
        Check.NotNull(batchWriteItemRequest, nameof(batchWriteItemRequest));

        using var activity = ActivityRepository.StartActivity();

        var attempts = 0;
        var remainingItems = batchWriteItemRequest;

        do
        {
            var items = remainingItems;

            var response = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(ct =>
                    DynamoClient.BatchWriteItemAsync(items, ct), cancellationToken)
                .ConfigureAwait(false);

            remainingItems = response.UnprocessedItems;

            if (!response.UnprocessedItems.HasItems())
                break;

            attempts++;

            activity?.SetTag(ConsumedCapacity, response.ConsumedCapacity);
            activity?.SetTag(StatusCode, response.HttpStatusCode);
            activity?.SetTag(RequestId, response.ResponseMetadata.RequestId);
        } while (attempts < maxRetry);

        return remainingItems;
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
        ArgumentNullException.ThrowIfNull(request);

        using var activity = ActivityRepository.StartActivity();

        var queryRequest = QueryHelper.CreateQueryRequest<T>(request, context);
        activity?.SetTag("TableName", queryRequest.TableName);
        activity?.SetTag("Page", request.Page);
        activity?.SetTag("PageSize", request.PageSize);
        activity?.SetTag("BatchSize", queryRequest.Limit);
        activity?.SetTag("ConsistentRead", queryRequest.ConsistentRead);

        var items = new List<Dictionary<string, AttributeValue>>();
        var remaining = request.PageSize;
        
        Dictionary<string, AttributeValue> lastEvaluatedKey = null;
        var quantityRequests = 0;
        do
        {   
            quantityRequests++;
                
            var response = await CreateDefaultRetryAsyncPolicy()
                .ExecuteAsync(ct => DynamoClient.QueryAsync(queryRequest, ct), cancellationToken)
                .ConfigureAwait(false);

            if (response == null)
                break;

            activity?.SetTag("HttpStatusCode", response.HttpStatusCode);
            activity?.SetTag("RequestId", response.ResponseMetadata?.RequestId);

            items.AddRange(response.Items);

            queryRequest.ExclusiveStartKey = lastEvaluatedKey = response.LastEvaluatedKey;
            remaining = remaining.HasValue ? request.PageSize - items.Count : 0;
            activity?.SetTag("Remaining", remaining);
            activity?.SetTag("QuantityRequests", quantityRequests);
            
        } while (ShouldContinue(lastEvaluatedKey, remaining));

        return (lastEvaluatedKey, items);
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
        ArgumentNullException.ThrowIfNull(request);

        using var activity = ActivityRepository.StartActivity();

        var scanRequest = QueryHelper.CreateScanRequest<T>(request, context);
        activity?.SetTag("TableName", scanRequest.TableName);
        activity?.SetTag("Page", request.Page);
        activity?.SetTag("PageSize", request.PageSize);

        Dictionary<string, AttributeValue> lastEvaluatedKey = null;

        var items = new List<T>();
        var remaining = request.PageSize;

        do
        {
            var response = await CreateDefaultRetryAsyncPolicy()
                .ExecuteAsync(ct => DynamoClient.ScanAsync(scanRequest, ct), cancellationToken)
                .ConfigureAwait(false);

            if (response == null)
                break;

            items.AddRange(QueryHelper.ConvertAttributesToType<T>(response.Items, context));
            scanRequest.ExclusiveStartKey = lastEvaluatedKey = response.LastEvaluatedKey;
            remaining = remaining.HasValue ? request.PageSize - items.Count : 0;

            activity?.SetTag("Remaining", remaining);

            if (remaining > 0) scanRequest.Limit = remaining.Value;
        } while (ShouldContinue(lastEvaluatedKey, remaining));

        return (lastEvaluatedKey, items);
    }

    /// <summary>
    ///     Just evaluates if the last evaluated key is not null and the remaining items are greater than 0
    /// </summary>
    /// <param name="lastEvaluatedKey"></param>
    /// <param name="remaining"></param>
    /// <returns></returns>
    private static bool ShouldContinue(Dictionary<string, AttributeValue> lastEvaluatedKey, int? remaining)
    {
        return lastEvaluatedKey?.Count > 0 && remaining > 0;
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
    ///     Disposes the DynamoDB context and AmazonDynamoDBClient resources.
    /// </summary>
    protected override void DisposeServices()
    {
        dynamoClient?.Dispose();
    }

    #region [Constructors]

    /// <summary>
    ///     Initializes a new instance of the <see cref="Repository" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="configuration">AWS configuration.</param>
    protected Repository(ILogger logger, IAwsConfiguration configuration) : base(logger, configuration)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Repository" /> class using a context map.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    /// <param name="context"></param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Repository(ILogger logger, IAwsConfiguration configuration, DynamoContext context) : base(logger,
        configuration)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
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

    #endregion


    #region [Add]

    /// <inheritdoc />
    public async Task AddAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : class
    {
        Check.NotNull(message, nameof(message));
        ThrowIfInstanceIsCollection(message);

        using var activity = ActivityRepository.StartActivity();

        var putRequest = new PutItemRequest
        {
            TableName = TableHelper.GetTableName<T>(context),
            Item = AttributeConverter.ConvertToAttributeValueMap(message, context)
        };

        var response = await CreateDefaultRetryAsyncPolicy()
            .ExecuteAsync(ct => DynamoClient.PutItemAsync(putRequest, ct), cancellationToken)
            .ConfigureAwait(false);

        activity?.SetTag(ConsumedCapacity, response.ConsumedCapacity);
        activity?.SetTag(StatusCode, response.HttpStatusCode);
        activity?.SetTag(RequestId, response.ResponseMetadata.RequestId);
    }

    /// <summary>
    ///     Adds a list of items to the DynamoDB table.
    /// </summary>
    /// <typeparam name="T">The type of items to add.</typeparam>
    /// <param name="messages">The list of items to add.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <exception cref="ArgumentNullException">Thrown if the messages parameter is null.</exception>
    public async Task AddRangeAsync<T>(ICollection<T> messages, CancellationToken cancellationToken = default)
        where T : class
    {
        Check.NotNull(messages, nameof(messages));
        
        using var activity = ActivityRepository.StartActivity();
        activity?.SetTag("Messages", messages.Count);

        // No messages but the log will be created
        if(messages.Count == 0)
            return;
        
        var tableName = TableHelper.GetTableName<T>(context);

        var writeRequest = messages.Select(message => new WriteRequest
            {
                PutRequest = new PutRequest
                {
                    Item = AttributeConverter.ConvertToAttributeValueMap(message, context)
                }
            })
            .ToList();

        var batchRequest = new Dictionary<string, List<WriteRequest>> { { tableName, writeRequest } };

        var response = await InternalBatchWriteItemAsync(batchRequest, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (response.HasItems())
            throw new CriticalException("Some items could not be added to the table");
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
        Check.NotNull(request, nameof(request));

        if (request.TransactItems is null || request.TransactItems.Count is > 25 or 0)
            throw new BusinessException(
                "The number of transactItems should be greater than 0 and less or equal than 25");

        using var activity = ActivityRepository.StartActivity();
        activity?.SetTag("TransactWriteItems", request.TransactItems.Count);

        var transactRequest = new TransactWriteItemsRequest
        {
            ClientRequestToken = request.ClientRequestToken,
            TransactItems = []
        };

        foreach (var transactItem in request.TransactItems)
            transactRequest.TransactItems.Add(QueryHelper.CreateTransactionWriteItem(transactItem));

        var response = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(
                ct => DynamoClient.TransactWriteItemsAsync(transactRequest, ct), cancellationToken)
            .ConfigureAwait(false);

        activity?.SetTag(ConsumedCapacity, response.ConsumedCapacity);
        activity?.SetTag(StatusCode, response.HttpStatusCode);
        activity?.SetTag(RequestId, response.ResponseMetadata.RequestId);
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
        Check.NotNull(batchWriteItemRequest, nameof(batchWriteItemRequest));

        using (ActivityRepository.StartActivity())
        {
            var request = QueryHelper.CreateBatchWriteItemRequest(batchWriteItemRequest, context);
            var result = new BatchWriteItemResponse();
            var attempts = 0;
            var backOfficeRandom = new Random(1);

            do
            {
                var response = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(
                        ct => DynamoClient.BatchWriteItemAsync(request, ct), cancellationToken)
                    .ConfigureAwait(false);

                if (response.UnprocessedItems.IsNullOrEmpty())
                    return result;

                request.RequestItems = response.UnprocessedItems;
                attempts++;

                //sleeps for a while before retrying
                if (request.RequestItems.Count != 0)
                    Thread.Sleep(TimeSpan.FromSeconds(batchWriteItemRequest.RetryDelay.Seconds *
                                                      backOfficeRandom.Next(1, 3)));
            } while (attempts < batchWriteItemRequest.MaxRetry);

            return result;
        }
    }

    #endregion

    #region [Delete]

    /// <inheritdoc />
    public async Task DeleteAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : class
    {
        Check.NotNull(message, nameof(message));
        ThrowIfInstanceIsCollection(message);

        using (ActivityRepository.StartActivity())
        {
            var keys = TableHelper.GetKeyValues(message, context);

            await DeleteAsync<T>(keys.HashKey, keys.RangeKey?.ToString(), cancellationToken).ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync<T>(object id, string rangeKey = null, CancellationToken cancellationToken = default)
        where T : class
    {
        Check.NotNull(id, nameof(id));

        using var activity = ActivityRepository.StartActivity();
        activity?.SetTag("Id", id);
        activity?.SetTag("RangeKey", rangeKey);

        var deleteRequest = new DeleteItemRequest
        {
            TableName = TableHelper.GetTableName<T>(context),
            Key = TableHelper.ParseKeysToAttributeValueMap<T>(id, rangeKey, context)
        };

        var response = await CreateDefaultRetryAsyncPolicy()
            .ExecuteAsync(ct => DynamoClient.DeleteItemAsync(deleteRequest, ct), cancellationToken)
            .ConfigureAwait(false);

        activity?.SetTag(ConsumedCapacity, response.ConsumedCapacity);
        activity?.SetTag(StatusCode, response.HttpStatusCode);
        activity?.SetTag(RequestId, response.ResponseMetadata.RequestId);
    }

    /// <inheritdoc />
    public async Task DeleteRangeAsync<T>(ICollection<T> messages, CancellationToken cancellationToken = default)
        where T : class
    {
        Check.NotNull(messages, nameof(messages));

        using var activity = ActivityRepository.StartActivity();
        activity?.SetTag("Messages", messages.Count);

        // No messages but the log will be created
        if(messages.Count == 0)
            return;
        
        var tableName = TableHelper.GetTableName<T>(context);
        activity?.SetTag("TableName", tableName);
        
        var writeRequest = messages.Select(message => new WriteRequest
        {
            DeleteRequest =
                new DeleteRequest
                {
                    Key = TableHelper.ExtractKeyAttributeValueMap(message, context)
                }
        }).ToList();

        var response = await InternalBatchWriteItemAsync(new Dictionary<string, List<WriteRequest>>
        {
            { tableName, writeRequest }
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

        activity?.SetTag("UnprocessedItems", response.Count);
        
        if (response.HasItems())
            throw new CriticalException("Some items could not be deleted from the table");
    }

    #endregion

    #region [Update]

    /// <exception cref="ArgumentNullException"></exception>
    /// <inheritdoc />
    public async Task<T> UpdateAsync<T>(T instance, CancellationToken cancellationToken = default)
        where T : class, new()
    {
        Check.NotNull(instance, nameof(instance));

        using (ActivityRepository.StartActivity())
        {
            var tableName = TableHelper.GetTableName<T>(context);

            var keys = TableHelper.ExtractKeyAttributeValueMap(instance, context);

            var attributesToUpdate = AttributeConverter.ConvertToAttributeValueMap(instance, context);

            if (attributesToUpdate is null) throw new CriticalException("Attributes to update is null");

            //Remove keys from attributes to update
            foreach (var key in keys) attributesToUpdate.Remove(key.Key);

            var updateItemRequest = new UpdateItemRequest
            {
                TableName = tableName,
                Key = keys,
                //Converts to a dictionary of AttributeValueUpdate
                AttributeUpdates = attributesToUpdate.ToDictionary(x => x.Key, x => new AttributeValueUpdate
                {
                    Action = AttributeAction.PUT,
                    Value = x.Value
                })
            };

            //If the user request a return value
            var newInstance = await UpdateAsync<T>(updateItemRequest, cancellationToken).ConfigureAwait(false);

            return newInstance ?? instance;
        }
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
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(attributeUpdates);

        using var activity = ActivityRepository.StartActivity();

        var response = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(ct =>
                DynamoClient.UpdateItemAsync(tableName, key, attributeUpdates, ct), cancellationToken)
            .ConfigureAwait(false);

        activity?.SetTag(ConsumedCapacity, response.ConsumedCapacity);
        activity?.SetTag(StatusCode, response.HttpStatusCode);
        activity?.SetTag(RequestId, response.ResponseMetadata.RequestId);
    }

    /// <summary>
    ///     Updates an item in the DynamoDB table based on the provided update item request.
    /// </summary>
    /// <typeparam name="T">The type of item to update.</typeparam>
    /// <param name="updateItemRequest">The update item request.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>The updated item of type T.</returns>
    /// <exception cref="ArgumentNullException">Thrown when updateItemRequest is null.</exception>
    private async Task<T> UpdateAsync<T>(UpdateItemRequest updateItemRequest,
        CancellationToken cancellationToken = default) where T : class, new()
    {
        Check.NotNull(updateItemRequest, nameof(updateItemRequest));

        using var activity = ActivityRepository.StartActivity();
        activity?.SetTag("TableName", updateItemRequest.TableName);

        var response = await CreateDefaultRetryAsyncPolicy()
            .ExecuteAsync(ct => DynamoClient.UpdateItemAsync(updateItemRequest, ct), cancellationToken)
            .ConfigureAwait(false);

        activity?.SetTag(ConsumedCapacity, response.ConsumedCapacity);
        activity?.SetTag(StatusCode, response.HttpStatusCode);
        activity?.SetTag(RequestId, response.ResponseMetadata.RequestId);

        return response.Attributes.IsNullOrEmpty()
            ? default
            : AttributeConverter.ConvertAttributeValuesToType<T>(response.Attributes, context);
    }

    #endregion

    #region [Queries]

    /// <inheritdoc />
    public async Task<T> QueryFirstAsync<T>(object id, CancellationToken cancellationToken = default)
        where T : class
    {
        using (ActivityRepository.StartActivity())
        {
            var queryRequest = new QueryRequest
            {
                KeyConditionExpression = TableHelper.BuildDefaultKeyExpression<T>(false, context),
                Filter = new
                {
                    pk = id
                }
            };

            var result = await QueryAsync<T>(queryRequest, cancellationToken).ConfigureAwait(false);

            return result?.FirstOrDefault();
        }
    }

    /// <inheritdoc />
    public async Task<T> GetByIdAsync<T>(object id, string rangeKey = null,
        CancellationToken cancellationToken = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(id);

        using (ActivityRepository.StartActivity())
        {
            var queryRequest = new QueryRequest
            {
                KeyConditionExpression = TableHelper.BuildDefaultKeyExpression<T>(rangeKey != null, context),
                Filter = new
                {
                    pk = id,
                    sk = rangeKey
                }
            };

            var result = await QueryAsync<T>(queryRequest, cancellationToken).ConfigureAwait(false);

            return result?.SingleOrDefault();
        }
    }

    /// <summary>
    ///     Queries the DynamoDB table and returns a list of items with the specified id.
    /// </summary>
    /// <typeparam name="T">The type of items to query.</typeparam>
    /// <param name="id">The id to query.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A list of items with the specified id.</returns>
    public async Task<IList<T>> QueryAsync<T>(object id, CancellationToken cancellationToken = default)
        where T : class
    {
        using (ActivityRepository.StartActivity())
        {
            var queryRequest = new QueryRequest
            {
                KeyConditionExpression = TableHelper.BuildDefaultKeyExpression<T>(false, context),
                Filter = new
                {
                    pk = id
                }
            };

            return await QueryAsync<T>(queryRequest, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     Queries the DynamoDB table using a query request and returns a list of items.
    /// </summary>
    /// <typeparam name="T">The type of items to query.</typeparam>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A list of items based on the query request.</returns>
    public async Task<IList<T>> QueryAsync<T>(QueryRequest request, CancellationToken cancellationToken = default)
        where T : class
    {
        using var activity = ActivityRepository.StartActivity();

        ArgumentNullException.ThrowIfNull(request);

        var (_, response) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

        return QueryHelper.ConvertAttributesToType<T>(response, context);
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
        where TResult1 : class
        where TResult2 : class
    {
        ArgumentNullException.ThrowIfNull(request);

        using (ActivityRepository.StartActivity())
        {
            var (_, items) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

            return QueryHelper.ConvertAttributesToType<TResult1, TResult2>(items, splitBy, context);
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
        where TResult1 : class
        where TResult2 : class
        where TResult3 : class
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(splitBy);

        using (ActivityRepository.StartActivity())
        {
            var (_, items) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

            return QueryHelper.ConvertAttributesToType<TResult1, TResult2, TResult3>(items, splitBy, context);
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
        where TResult1 : class
        where TResult2 : class
        where TResult3 : class
        where TResult4 : class
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(splitBy);

        using (ActivityRepository.StartActivity())
        {
            var (_, items) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

            return QueryHelper
                .ConvertAttributesToType<TResult1, TResult2, TResult3, TResult4>(items, splitBy, context);
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
        where TResult1 : class
        where TResult2 : class
        where TResult3 : class
        where TResult4 : class
        where TResult5 : class
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(splitBy);

        using (ActivityRepository.StartActivity())
        {
            var (_, items) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

            return QueryHelper.ConvertAttributesToType<TResult1, TResult2, TResult3, TResult4, TResult5>(items,
                splitBy, context);
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
        ArgumentNullException.ThrowIfNull(request);
        
        using var activity = ActivityRepository.StartActivity();
        request.PageSize = 1;
        request.Page = null;

        var (_, items) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);
        
        var queryResponse = QueryHelper.ConvertAttributesToType<T>(items, context);

        return queryResponse.FirstOrDefault();
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
        ArgumentNullException.ThrowIfNull(request);

        using (ActivityRepository.StartActivity())
        {
            var (lastEvaluatedKey, items) =
                await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

            return new PagedCollection<T>
            {
                Items = QueryHelper.ConvertAttributesToType<T>(items, context),
                Page = QueryHelper.CreatePaginationToken(lastEvaluatedKey),
                PageSize = request.PageSize.GetValueOrDefault()
            };
        }
    }

    #endregion

    #region [Scans]

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
            var response = await InternalScanAsync<T>(request, cancellationToken).ConfigureAwait(false);

            //This implementation hides pagination and returns all items

            return response.Items;
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
        ArgumentNullException.ThrowIfNull(request);

        using (ActivityRepository.StartActivity())
        {
            var (exclusiveStartKey, items) =
                await InternalScanAsync<T>(request, cancellationToken).ConfigureAwait(false);

            if (items?.Count == 0)
                return new PagedCollection<T>();

            var response = new PagedCollection<T>
            {
                Items = items,
                Page = QueryHelper.CreatePaginationToken(exclusiveStartKey),
                PageSize = request.PageSize.GetValueOrDefault()
            };

            return response;
        }
    }

    #endregion
    
    /// <summary>
    /// Throw an exception if the instance is a collection.
    /// </summary>
    /// <param name="message"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="CriticalException"></exception>
    private static void ThrowIfInstanceIsCollection<T>(T message)
    {
        if (TypeUtil.IsCollection(message))
            throw new CriticalException("You should use AddRangeAsync to add a list of items");
    }
}