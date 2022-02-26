// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Dynamo
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.Table;
using Innovt.Core.Collections;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using QueryRequest = Innovt.Cloud.Table.QueryRequest;
using ScanRequest = Innovt.Cloud.Table.ScanRequest;


namespace Innovt.Cloud.AWS.Dynamo
{
    public abstract class Repository : AwsBaseService, ITableRepository
    {
        private static readonly ActivitySource ActivityRepository = new("Innovt.Cloud.AWS.Dynamo.Repository");

        private DynamoDBContext context;
        private AmazonDynamoDBClient dynamoClient;

        protected Repository(ILogger logger, IAwsConfiguration configuration) : base(logger, configuration)
        {
        }

        protected Repository(ILogger logger, IAwsConfiguration configuration, string region) : base(logger,
            configuration, region)
        {
        }

        private DynamoDBContext Context => context ??= new DynamoDBContext(DynamoClient);
        private AmazonDynamoDBClient DynamoClient => dynamoClient ??= CreateService<AmazonDynamoDBClient>();

        private static DynamoDBOperationConfig OperationConfig =>
            new()
            {
                ConsistentRead = true,
                Conversion = DynamoDBEntryConversion.V2
            };

        public async Task<T> GetByIdAsync<T>(object id, string rangeKey = null,
            CancellationToken cancellationToken = default) where T : ITableMessage
        {
            using (ActivityRepository.StartActivity(nameof(GetByIdAsync)))
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

        public async Task DeleteAsync<T>(T value, CancellationToken cancellationToken = default) where T : ITableMessage
        {
            using (ActivityRepository.StartActivity(nameof(DeleteAsync)))
            {
                await CreateDefaultRetryAsyncPolicy()
                    .ExecuteAsync(async () => await Context.DeleteAsync(value, cancellationToken).ConfigureAwait(false))
                    .ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync<T>(object id, string rangeKey = null,
            CancellationToken cancellationToken = default) where T : ITableMessage
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            using var activity = ActivityRepository.StartActivity(nameof(DeleteAsync));
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

        public async Task AddAsync<T>(T message, CancellationToken cancellationToken = default) where T : ITableMessage
        {
            using (ActivityRepository.StartActivity(nameof(DeleteAsync)))
            {
                await CreateDefaultRetryAsyncPolicy()
                    .ExecuteAsync(async () =>
                        await Context.SaveAsync(message, OperationConfig, cancellationToken).ConfigureAwait(false))
                    .ConfigureAwait(false);
            }
        }

        public async Task AddAsync<T>(IList<T> messages, CancellationToken cancellationToken = default)
            where T : ITableMessage
        {
            if (messages is null) throw new ArgumentNullException(nameof(messages));

            using (ActivityRepository.StartActivity(nameof(AddAsync)))
            {
                var batch = Context.CreateBatchWrite<T>(OperationConfig);

                batch.AddPutItems(messages);
                await CreateDefaultRetryAsyncPolicy()
                    .ExecuteAsync(async () => await batch.ExecuteAsync(cancellationToken).ConfigureAwait(false))
                    .ConfigureAwait(false);
            }
        }

        public async Task<T> QueryFirstAsync<T>(object id, CancellationToken cancellationToken = default)
        {
            using (ActivityRepository.StartActivity(nameof(QueryFirstAsync)))
            {
                var result = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                        await Context.QueryAsync<T>(id, OperationConfig).GetNextSetAsync(cancellationToken)
                            .ConfigureAwait(false))
                    .ConfigureAwait(false);

                return result == null ? default : result.FirstOrDefault();
            }
        }

        public async Task<IList<T>> QueryAsync<T>(object id, CancellationToken cancellationToken = default)
        {
            using (ActivityRepository.StartActivity(nameof(QueryAsync)))
            {
                var result = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                        await Context.QueryAsync<T>(id, OperationConfig).GetRemainingAsync(cancellationToken)
                            .ConfigureAwait(false))
                    .ConfigureAwait(false);

                return result;
            }
        }

        public async Task<IList<T>> QueryAsync<T>(QueryRequest request, CancellationToken cancellationToken = default)
        {
            using (ActivityRepository.StartActivity(nameof(QueryAsync)))
            {
                if (request is null) throw new ArgumentNullException(nameof(request));

                var (_, items) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

                return Helpers.ConvertAttributesToType<T>(items, Context);
            }
        }

        public async Task<(IList<TResult1> first, IList<TResult2> second)> QueryMultipleAsync<T, TResult1, TResult2>(
            QueryRequest request, string splitBy, CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            using (ActivityRepository.StartActivity(nameof(QueryMultipleAsync)))
            {
                var (_, items) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

                return Helpers.ConvertAttributesToType<TResult1, TResult2>(items, splitBy, Context);
            }
        }

        public async Task<(IList<TResult1> first, IList<TResult2> second, IList<TResult3> third)>
            QueryMultipleAsync<T, TResult1, TResult2, TResult3>(QueryRequest request, string[] splitBy,
                CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (splitBy == null) throw new ArgumentNullException(nameof(splitBy));

            using (ActivityRepository.StartActivity(nameof(QueryMultipleAsync)))
            {
                var (_, items) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

                return Helpers.ConvertAttributesToType<TResult1, TResult2, TResult3>(items, splitBy, Context);
            }
        }

        public async Task<(IList<TResult1> first, IList<TResult2> second, IList<TResult3> third, IList<TResult4> fourth)>
          QueryMultipleAsync<T, TResult1, TResult2, TResult3, TResult4>(QueryRequest request, string[] splitBy,
              CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (splitBy == null) throw new ArgumentNullException(nameof(splitBy));

            using (ActivityRepository.StartActivity(nameof(QueryMultipleAsync)))
            {
                var (_, items) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

                return Helpers.ConvertAttributesToType<TResult1, TResult2, TResult3, TResult4>(items, splitBy, Context);
            }
        }

        public async Task<(IList<TResult1> first, IList<TResult2> second, IList<TResult3> third, IList<TResult4> fourth, IList<TResult5> fifth)>
       QueryMultipleAsync<T, TResult1, TResult2, TResult3, TResult4, TResult5>(QueryRequest request, string[] splitBy,
           CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (splitBy == null) throw new ArgumentNullException(nameof(splitBy));

            using (ActivityRepository.StartActivity(nameof(QueryMultipleAsync)))
            {
                var (_, items) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

                return Helpers.ConvertAttributesToType<TResult1, TResult2, TResult3, TResult4, TResult5>(items, splitBy, Context);
            }
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(QueryRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            using (ActivityRepository.StartActivity(nameof(QueryFirstOrDefaultAsync)))
            {
                request.PageSize = 1;
                request.Page = null;

                var (_, items) = await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

                var queryResponse = Helpers.ConvertAttributesToType<T>(items, Context);

                return queryResponse.FirstOrDefault();
            }
        }

        public async Task<PagedCollection<T>> QueryPaginatedByAsync<T>(QueryRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            using (ActivityRepository.StartActivity(nameof(QueryFirstOrDefaultAsync)))
            {
                var (lastEvaluatedKey, items) =
                    await InternalQueryAsync<T>(request, cancellationToken).ConfigureAwait(false);

                return new PagedCollection<T>
                {
                    Items = Helpers.ConvertAttributesToType<T>(items, Context),
                    Page = Helpers.CreatePaginationToken(lastEvaluatedKey),
                    PageSize = request.PageSize.GetValueOrDefault()
                };
            }
        }

        public async Task TransactWriteItemsAsync(TransactionWriteRequest request, CancellationToken cancellationToken)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            if (request.TransactItems is null || (request.TransactItems.Count is > 25 or 0))
                throw new BusinessException("The number of transactItems should be greater than 0 and less or equal than 25");

            using var activity = ActivityRepository.StartActivity(nameof(TransactWriteItemsAsync));
            activity?.SetTag("TransactWriteItems", request.TransactItems.Count);

            var transactRequest = new TransactWriteItemsRequest()
            {
                ClientRequestToken = request.ClientRequestToken
            };

            foreach (var transactItem in request.TransactItems)
            {
                transactRequest.TransactItems.Add(Helpers.CreateTransactionWriteItem(transactItem));
            }

            await DynamoClient.TransactWriteItemsAsync(transactRequest, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IList<T>> ScanAsync<T>(ScanRequest request,
            CancellationToken cancellationToken = default)
        {
            using (ActivityRepository.StartActivity(nameof(ScanAsync)))
            {
                return (await InternalScanAsync<T>(request, cancellationToken).ConfigureAwait(false)).Items;
            }
        }


        public async Task<PagedCollection<T>> ScanPaginatedByAsync<T>(ScanRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            using (ActivityRepository.StartActivity(nameof(ScanPaginatedByAsync)))
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

        protected override AsyncRetryPolicy CreateDefaultRetryAsyncPolicy()
        {
            return base.CreateRetryAsyncPolicy<ProvisionedThroughputExceededException,
                InternalServerErrorException, LimitExceededException, ResourceInUseException>();
        }

        protected async Task UpdateAsync(string tableName, Dictionary<string, AttributeValue> key,
            Dictionary<string, AttributeValueUpdate> attributeUpdates, CancellationToken cancellationToken = default)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (attributeUpdates is null)
            {
                throw new ArgumentNullException(nameof(attributeUpdates));
            }

            using (ActivityRepository.StartActivity(nameof(UpdateAsync)))
            {
                await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () => await DynamoClient.UpdateItemAsync(tableName, key, attributeUpdates, cancellationToken).ConfigureAwait(false))
                    .ConfigureAwait(false);
            }
        }

        protected async Task<T> UpdateAsync<T>(UpdateItemRequest updateItemRequest,CancellationToken cancellationToken = default)
        {   
            if (updateItemRequest is null)
                throw new ArgumentNullException(nameof(updateItemRequest));

            using (ActivityRepository.StartActivity(nameof(UpdateAsync)))
            {
                var response = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await DynamoClient.UpdateItemAsync(updateItemRequest, cancellationToken).ConfigureAwait(false)).ConfigureAwait(false);

                if(response.Attributes is null)
                    return default;

                var doc = Document.FromAttributeMap(response.Attributes);

                if (doc is null)
                    return default;

                return Context.FromDocument<T>(doc);
            }        
        }


        public async Task<ExecuteSqlStatementResponse<T>> ExecuteStatementAsync<T>(ExecuteSqlStatementRequest sqlStatementRequest, CancellationToken cancellationToken = default) where T:class
        {
            if (sqlStatementRequest is null)
            {
                throw new ArgumentNullException(nameof(sqlStatementRequest));
            }

            using (ActivityRepository.StartActivity(nameof(UpdateAsync)))
            {
                var response = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>

                await DynamoClient.ExecuteStatementAsync(new ExecuteStatementRequest()
                { 
                    ConsistentRead = sqlStatementRequest.ConsistentRead,
                    NextToken = sqlStatementRequest.NextToken,
                    Statement = sqlStatementRequest.Statment
                }).ConfigureAwait(false)).ConfigureAwait(false);


                if(response is null)
                    return null;

                return new ExecuteSqlStatementResponse<T>()
                {
                    NextToken = sqlStatementRequest.NextToken,
                    Items = Helpers.ConvertAttributesToType<T>(response.Items, Context)
                };
            }
        }

        private async Task<(Dictionary<string, AttributeValue> LastEvaluatedKey, IList<Dictionary<string, AttributeValue>> Items)> InternalQueryAsync<T>(QueryRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            using (ActivityRepository.StartActivity(nameof(InternalQueryAsync)))
            {
                var queryRequest = Helpers.CreateQueryRequest<T>(request);

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


        private async Task<(Dictionary<string, AttributeValue> ExclusiveStartKey, IList<T> Items)> InternalScanAsync<T>(
            ScanRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            using (ActivityRepository.StartActivity(nameof(InternalScanAsync)))
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

                    items.AddRange(Helpers.ConvertAttributesToType<T>(iterator.Current.Items, Context));
                    scanRequest.ExclusiveStartKey = lastEvaluatedKey = iterator.Current.LastEvaluatedKey;
                    remaining = remaining.HasValue ? request.PageSize - items.Count : 0;

                    if (remaining > 0) scanRequest.Limit = remaining.Value;
                } while (lastEvaluatedKey.Count > 0 && remaining > 0);

                return (lastEvaluatedKey, items);
            }
        }

        protected override void DisposeServices()
        {
            context?.Dispose();
            dynamoClient?.Dispose();
        }
    }
}