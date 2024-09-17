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
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Innovt.Cloud.AWS.Configuration;
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
    private const string RequestId = "requestId";
    private const string ConsumedCapacity = "consumedCapacity";
    private const string StatusCode = "statusCode";
    
    private static readonly ActivitySource ActivityRepository = new("Innovt.Cloud.AWS.Dynamo.Repository");
    
    private AmazonDynamoDBClient dynamoClient;
    private readonly DynamoContext context;

    #region [Constructors]
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
    #endregion

    #region [Configuration]
    /// <summary>
    ///     Gets the Amazon DynamoDB client.
    /// </summary>
    private AmazonDynamoDBClient DynamoClient => dynamoClient ??= CreateService<AmazonDynamoDBClient>();
    #endregion

    #region [Add]
    /// <inheritdoc />
    public async Task AddAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : class, ITableMessage
    {
        Check.NotNull(message, nameof(message));
        
        using var activity = ActivityRepository.StartActivity();
     
        var putRequest = new PutItemRequest
        {
            TableName = DynamoHelper.GetTableName<T>(context),
            Item = AttributeConverter.ConvertTypeToAttributes(message,context)
        };
        
        var resp = await CreateDefaultRetryAsyncPolicy()
            .ExecuteAsync(async () =>
                await dynamoClient.PutItemAsync(putRequest, cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);

        activity?.SetTag(ConsumedCapacity, resp.ConsumedCapacity);
        activity?.SetTag(StatusCode, resp.HttpStatusCode);
        activity?.SetTag(RequestId, resp.ResponseMetadata.RequestId);
    }

    /// <summary>
    ///     Adds a list of items to the DynamoDB table.
    /// </summary>
    /// <typeparam name="T">The type of items to add.</typeparam>
    /// <param name="messages">The list of items to add.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <exception cref="ArgumentNullException">Thrown if the messages parameter is null.</exception>
    public async Task AddAsync<T>(IList<T> messages, CancellationToken cancellationToken = default)
        where T : class
    {
        Check.NotNull(messages, nameof(messages));
        using var activity = ActivityRepository.StartActivity();
        activity?.SetTag("messages", messages.Count);
        
        var tableName = DynamoHelper.GetTableName<T>(context);

        var writeRequest = messages.Select(message => new WriteRequest { PutRequest = new PutRequest { Item = AttributeConverter.ConvertTypeToAttributes(message, context) } }).ToList();

        var batchRequest = new Dictionary<string, List<WriteRequest>> { { tableName, writeRequest } };

        var response = await InternalBatchWriteItemAsync(batchRequest, cancellationToken:cancellationToken).ConfigureAwait(false);
        
        if(response.HasItems())
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
            transactRequest.TransactItems.Add(DynamoHelper.CreateTransactionWriteItem(transactItem));

        await DynamoClient.TransactWriteItemsAsync(transactRequest, cancellationToken).ConfigureAwait(false);
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
            var request = DynamoHelper.CreateBatchWriteItemRequest(batchWriteItemRequest,context);
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

                //sleeps for a while before retrying
                if(request.RequestItems.Count != 0)
                    Thread.Sleep(TimeSpan.FromSeconds(batchWriteItemRequest.RetryDelay.Seconds * backOfficeRandom.Next(1, 3)));
                
            } while (attempts < batchWriteItemRequest.MaxRetry);

            return result;
        }
    }

     

    #endregion
    
    #region [Delete]
    
    /// <inheritdoc />
    public async Task DeleteAsync<T>(T message, CancellationToken cancellationToken = default) where T : class, ITableMessage
    {
        Check.NotNull(message, nameof(message));
        
        using (ActivityRepository.StartActivity())
        {
            var keys = DynamoHelper.GetKeyValues(message, context);
            
            await DeleteAsync<T>(keys.HashKey, keys.RangeKey?.ToString(), cancellationToken);
        }
    }
    public async Task DeleteAsync<T>(List<T> messages, CancellationToken cancellationToken = default) where T : class
    {
        Check.NotNull(messages, nameof(messages));
        
        using (ActivityRepository.StartActivity())
        {
            var tableName = DynamoHelper.GetTableName<T>(context);

            var writeRequest = messages.Select(message => new WriteRequest { DeleteRequest = 
                new DeleteRequest()
                {
                    Key = DynamoHelper.ExtractKeys(message, context)
                } }).ToList();
            
            var response = await InternalBatchWriteItemAsync(new Dictionary<string, List<WriteRequest>>
            {
                { tableName, writeRequest }
            }, cancellationToken:cancellationToken).ConfigureAwait(false);
        
            if(response.HasItems())
                throw new CriticalException("Some items could not be deleted from the table");
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync<T>(object id, string rangeKey = null, CancellationToken cancellationToken = default)
        where T : class, ITableMessage
    {
        Check.NotNull(id, nameof(id));

        using var activity = ActivityRepository.StartActivity();
        activity?.SetTag("id", id);
        activity?.SetTag("rangeKey", rangeKey);
        
        var deleteRequest = new DeleteItemRequest()
        {
            TableName = DynamoHelper.GetTableName<T>(context),
            Key = DynamoHelper.ExtractKeysToDictionary<T>(id,rangeKey, context),
        };
        
        var response = await CreateDefaultRetryAsyncPolicy()
            .ExecuteAsync(async () => await dynamoClient.DeleteItemAsync(deleteRequest,cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);
        
        activity?.SetTag(ConsumedCapacity, response.ConsumedCapacity);
        activity?.SetTag(StatusCode, response.HttpStatusCode);
        activity?.SetTag(RequestId, response.ResponseMetadata.RequestId);
    }
    #endregion

    #region [Update]
    /// <exception cref="ArgumentNullException"></exception>
    /// <inheritdoc />
    public async Task<T> UpdateAsync<T>(T instance, CancellationToken cancellationToken = default) where T:class
    {
        Check.NotNull(instance, nameof(instance));
      
        using (ActivityRepository.StartActivity())
        {
            var tableName = DynamoHelper.GetTableName<T>(context);
            var keys = DynamoHelper.ExtractKeys(instance, context);
            var attributesToUpdate = AttributeConverter.ConvertTypeToAttributes(instance, context);

            if(attributesToUpdate is null) throw new CriticalException("Attributes to update is null");
            
            //Remove keys from attributes to update
            foreach (var key in keys)
            {
                attributesToUpdate.Remove(key.Key);
            }
            
            var updateItemRequest = new UpdateItemRequest
            {
                TableName = tableName,
                Key = keys,
                //Converts to a dictionary of AttributeValueUpdate
                AttributeUpdates = attributesToUpdate.ToDictionary(x => x.Key, x => new AttributeValueUpdate()
                {
                    Action = AttributeAction.PUT,
                    Value = x.Value
                })
            };
            
            //If the user request a return value
            var newInstance =  await UpdateAsync<T>(updateItemRequest, cancellationToken);

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
        if (key is null) throw new ArgumentNullException(nameof(key));

        if (attributeUpdates is null) throw new ArgumentNullException(nameof(attributeUpdates));

        using var activity = ActivityRepository.StartActivity();
        
       var response =  await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await DynamoClient.UpdateItemAsync(tableName, key, attributeUpdates, cancellationToken)
                    .ConfigureAwait(false))
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
    protected async Task<T> UpdateAsync<T>(UpdateItemRequest updateItemRequest,
        CancellationToken cancellationToken = default)
    {
        Check.NotNull(updateItemRequest, nameof(updateItemRequest));

        using var activity = ActivityRepository.StartActivity();
        activity?.SetTag("tableName", updateItemRequest.TableName);
        
        var response = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await DynamoClient.UpdateItemAsync(updateItemRequest,
                    cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);

        activity?.SetTag(ConsumedCapacity, response.ConsumedCapacity);
        activity?.SetTag(StatusCode, response.HttpStatusCode);
        activity?.SetTag(RequestId, response.ResponseMetadata.RequestId);
        
        return response.Attributes.IsNullOrEmpty()
            ? default
            : AttributeConverter.ConvertAttributesToType<T>(response.Attributes,context);
    }

    #endregion
    
    #region [Queries] 
    /// <inheritdoc />
    public async Task<T> QueryFirstAsync<T>(object id, CancellationToken cancellationToken = default) where T : class
    {
        using (ActivityRepository.StartActivity())
        {
            var queryRequest = new QueryRequest
            {
                KeyConditionExpression = DynamoHelper.BuildDefaultKeyExpression<T>(false,context),
                Filter = new
                {
                    pk = id,
                }
            };

            var result = await QueryAsync<T>(queryRequest, cancellationToken).ConfigureAwait(false);
            
            return result?.FirstOrDefault();
        }
    }

    /// <inheritdoc />
    public async Task<T> GetByIdAsync<T>(object id, string rangeKey = null, CancellationToken cancellationToken = default) where T : class, ITableMessage
    {
        if (id == null) throw new ArgumentNullException(nameof(id));
        
        using (ActivityRepository.StartActivity())
        {  
            var queryRequest = new QueryRequest
            {
                KeyConditionExpression = DynamoHelper.BuildDefaultKeyExpression<T>(rangeKey!=null,context),
                Filter = new
                {
                    pk = id,
                    sk = rangeKey
                }
            };
                
            var result = await QueryAsync<T>(queryRequest, cancellationToken);

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
    public async Task<IList<T>> QueryAsync<T>(object id, CancellationToken cancellationToken = default) where T : class
    {
        using (ActivityRepository.StartActivity())
        {
            var queryRequest = new QueryRequest
            {
                KeyConditionExpression = DynamoHelper.BuildDefaultKeyExpression<T>(false, context),
                Filter = new
                {
                    pk = id
                }
            };

            return await QueryAsync<T>(queryRequest, cancellationToken);
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

            var (_, items) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);
            
            return DynamoHelper.ConvertAttributesToType<T>(items,context);
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

            return DynamoHelper.ConvertAttributesToType<TResult1, TResult2>(items, splitBy,context);
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

            return DynamoHelper.ConvertAttributesToType<TResult1, TResult2, TResult3>(items, splitBy,context);
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

            return DynamoHelper.ConvertAttributesToType<TResult1, TResult2, TResult3, TResult4>(items, splitBy,context);
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

            return DynamoHelper.ConvertAttributesToType<TResult1, TResult2, TResult3, TResult4, TResult5>(items, splitBy,context);
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

            var queryResponse = DynamoHelper.ConvertAttributesToType<T>(items,context);

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

        using (ActivityRepository.StartActivity())
        {
            var (lastEvaluatedKey, items) =
                await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

            return new PagedCollection<T>
            {
                Items = DynamoHelper.ConvertAttributesToType<T>(items,context),
                Page = DynamoHelper.CreatePaginationToken(lastEvaluatedKey),
                PageSize = request.PageSize.GetValueOrDefault()
            };
        }
    }
    #endregion

    #region  [Scans]
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
            var response  = await InternalScanAsync<T>(request, cancellationToken).ConfigureAwait(false);

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
                Page = DynamoHelper.CreatePaginationToken(exclusiveStartKey),
                PageSize = request.PageSize.GetValueOrDefault()
            };

            return response;
        }
    }
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
        ExecuteSqlStatementRequest sqlStatementRequest, CancellationToken cancellationToken = default) where T : class
    {
        if (sqlStatementRequest is null) throw new ArgumentNullException(nameof(sqlStatementRequest));

        using (ActivityRepository.StartActivity())
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
                Items = DynamoHelper.ConvertAttributesToType<T>(response.Items,context)
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
            var items = DynamoHelper.CreateBatchGetItemRequest(batchGetItemRequest);

            var response = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await DynamoClient.BatchGetItemAsync(items, cancellationToken).ConfigureAwait(false))
                .ConfigureAwait(false);

            if (response.Responses is null)
                return null;

            var result = new List<T>();

            foreach (var item in response.Responses) 
                result.AddRange(DynamoHelper.ConvertAttributesToType<T>(item.Value,context));

            return result;
        }
    }
    
    /// <summary>
    /// Execute a batch write item request returning the unprocessed items after some retries
    /// </summary>
    /// <param name="batchWriteItemRequest"></param>
    /// <param name="maxRetry"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, List<WriteRequest>>> InternalBatchWriteItemAsync(Dictionary<string, List<WriteRequest>> batchWriteItemRequest,
        int maxRetry = 3, CancellationToken cancellationToken = default)
    {
        Check.NotNull(batchWriteItemRequest, nameof(batchWriteItemRequest));

        using var activity = ActivityRepository.StartActivity();

        var attempts = 0;
        var remainingItems = batchWriteItemRequest;

        do
        {
            var items = remainingItems;

            var response = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await DynamoClient.BatchWriteItemAsync(items, cancellationToken).ConfigureAwait(false))
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
        if (request is null) throw new ArgumentNullException(nameof(request));

        using (ActivityRepository.StartActivity())
        {
            var queryRequest = DynamoHelper.CreateQueryRequest<T>(request,context);

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
            var scanRequest = DynamoHelper.CreateScanRequest<T>(request,context);

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

                items.AddRange(DynamoHelper.ConvertAttributesToType<T>(iterator.Current.Items,context));
                scanRequest.ExclusiveStartKey = lastEvaluatedKey = iterator.Current.LastEvaluatedKey;
                remaining = remaining.HasValue ? request.PageSize - items.Count : 0;

                if (remaining > 0) scanRequest.Limit = remaining.Value;
            } while (lastEvaluatedKey.Count > 0 && remaining > 0);

            return (lastEvaluatedKey, items);
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
    ///     Disposes the DynamoDB context and AmazonDynamoDBClient resources.
    /// </summary>
    protected override void DisposeServices()
    {
        dynamoClient?.Dispose();
    }
}