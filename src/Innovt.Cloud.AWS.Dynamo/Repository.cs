using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.Table;
using Innovt.Core.CrossCutting.Log;
using Polly.Retry;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Innovt.Core.Collections;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Dynamo
{
    public abstract class Repository : AwsBaseService, ITableRepository
    {
        protected Repository(ILogger logger, IAWSConfiguration configuration) : base(logger, configuration)
        {
        }

        protected Repository(ILogger logger, IAWSConfiguration configuration, string region) : base(logger, configuration, region)
        {
        }

        private DynamoDBContext context = null;
        private DynamoDBContext Context => context ??= new DynamoDBContext(DynamoClient);

        private AmazonDynamoDBClient dynamoClient = null;
        private AmazonDynamoDBClient DynamoClient => dynamoClient ??= CreateService<AmazonDynamoDBClient>();

        protected override AsyncRetryPolicy CreateDefaultRetryAsyncPolicy()
        {
            return base.CreateRetryAsyncPolicy<ProvisionedThroughputExceededException,
                                               InternalServerErrorException,LimitExceededException, ResourceInUseException>();
        }

        public async Task<T> GetByIdAsync<T>(object id, string rangeKey = null, CancellationToken cancellationToken = default) where T : ITableMessage
        {
            var config = new DynamoDBOperationConfig()
            {
                ConsistentRead = true
            };

            var policy = this.CreateDefaultRetryAsyncPolicy();

            if (string.IsNullOrEmpty(rangeKey))
                return await policy.ExecuteAsync(async () => await Context.LoadAsync<T>(id, config, cancellationToken)).ConfigureAwait(false);

            return await policy.ExecuteAsync(async () => await Context.LoadAsync<T>(id, rangeKey, config, cancellationToken)).ConfigureAwait(false);
        }

        public async Task DeleteAsync<T>(T value, CancellationToken cancellationToken = default) where T : ITableMessage
        {
            await this.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () => await Context.DeleteAsync<T>(value, cancellationToken)).ConfigureAwait(false);
        }

        public async Task DeleteAsync<T>(object id, string rangeKey=null, CancellationToken cancellationToken = default) where T : ITableMessage
        {
            var policy = this.CreateDefaultRetryAsyncPolicy();

            if (string.IsNullOrEmpty(rangeKey))
                await policy.ExecuteAsync(async () => await Context.DeleteAsync<T>(id, cancellationToken)).ConfigureAwait(false);
            else
                await policy.ExecuteAsync(async () => await Context.DeleteAsync<T>(id, rangeKey, cancellationToken)).ConfigureAwait(false);
        }

        public async Task AddAsync<T>(T message, CancellationToken cancellationToken = default) where T : ITableMessage
        {
            var config = new DynamoDBOperationConfig()
            {
                IgnoreNullValues = true
            };

            await this.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () => await Context.SaveAsync(message, config, cancellationToken)).ConfigureAwait(false);
        }

        public async Task AddAsync<T>(IList<T> messages, CancellationToken cancellationToken = default) where T : ITableMessage
        {
            if (messages is null) throw new System.ArgumentNullException(nameof(messages));
           
            var batch = Context.CreateBatchWrite<T>();

            batch.AddPutItems(messages);

            await this.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () => await batch.ExecuteAsync(cancellationToken)).ConfigureAwait(false);
        }

        protected async Task UpdateAsync(string tableName, Dictionary<string, AttributeValue> key,
            Dictionary<string, AttributeValueUpdate> attributeUpdates,
            CancellationToken cancellationToken = default)
        {
            await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () => 
            await DynamoClient.UpdateItemAsync(tableName, key, attributeUpdates, cancellationToken)).ConfigureAwait(false);
        }

        internal async Task<(Dictionary<string, AttributeValue> LastEvaluatedKey, IList<T> Items)> InternalQueryAsync<T>(Innovt.Cloud.Table.QueryRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var queryRequest = Helpers.CreateQueryRequest<T>(request);

            var queryResponse = await DynamoClient.QueryAsync(queryRequest).ConfigureAwait(false);

            return (queryResponse.LastEvaluatedKey, Helpers.ConvertAttributesToType<T>(queryResponse.Items, Context));
        }


        public async Task<T> QueryFirstAsync<T>(object id, CancellationToken cancellationToken = default)
        {
            var config = new DynamoDBOperationConfig()
            {
                BackwardQuery = true
            };
         

            var result = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () => 
                         await Context.QueryAsync<T>(id, config).GetNextSetAsync(cancellationToken)).ConfigureAwait(false);

            if (result == null)
                return default;

            return result.FirstOrDefault();
        }

    

        public async Task<IList<T>> QueryAsync<T>(object id, CancellationToken cancellationToken = default)
        {
            var config = new DynamoDBOperationConfig()
            {
                ConsistentRead = true,
                BackwardQuery = true,
            };

            var result = await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                          await Context.QueryAsync<T>(id, config).GetRemainingAsync(cancellationToken)).ConfigureAwait(false);

            return result;
        }
        public async Task<IList<T>> QueryAsync<T>(Table.QueryRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            return (await InternalQueryAsync<T>(request)).Items;
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(Table.QueryRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            request.PageSize = 1;

            var queryResponse = (await InternalQueryAsync<T>(request)).Items;
          
            return queryResponse.FirstOrDefault();
        }

        public async Task<PagedCollection<T>> QueryPaginatedByAsync<T>(Innovt.Cloud.Table.QueryRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var queryResponse = (await InternalQueryAsync<T>(request));


            return new PagedCollection<T>()
            {
                Items = queryResponse.Items,
                Page = Helpers.CreatePaginationToken(queryResponse.LastEvaluatedKey)
            };
        }


        internal async Task<(Dictionary<string,AttributeValue> ExclusiveStartKey, IList<T> Items)> InternalScanAsync<T>(Table.ScanRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var scanRequest = Helpers.CreateScanRequest<T>(request);

            var result = new List<T>();
            var pageSize = request.PageSize.GetValueOrDefault();

            do
            {
                if (pageSize > 0)
                {
                    // at least 10 elements per request
                    scanRequest.Limit = (pageSize - result.Count) < 10 ? 10 : (pageSize - result.Count);
                }

                var response = await DynamoClient.ScanAsync(scanRequest).ConfigureAwait(false);

                if (response.Items.Any())
                {
                    result.AddRange(Helpers.ConvertAttributesToType<T>(response.Items, Context));
                }

                scanRequest.ExclusiveStartKey = response.LastEvaluatedKey;

            } while (scanRequest.ExclusiveStartKey != null && scanRequest.ExclusiveStartKey.Count != 0 && (pageSize > 0 && (result.Count() < pageSize)));

            if(pageSize>0)
                return (scanRequest.ExclusiveStartKey, result.Take(pageSize).ToList());

            return (scanRequest.ExclusiveStartKey,result);
        }


        public async Task<IList<T>> ScanAsync<T>(Table.ScanRequest request, CancellationToken cancellationToken = default)
        {
            return (await InternalScanAsync<T>(request)).Items;
        }


        public async Task<PagedCollection<T>> ScanPaginatedByAsync<T>(Innovt.Cloud.Table.ScanRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var scanResponse = await this.InternalScanAsync<T>(request, cancellationToken);

            if (scanResponse.Items?.Count() ==0)
                return new PagedCollection<T>();

            var response = new PagedCollection<T>()
            {
                Items = scanResponse.Items,
                Page  = Helpers.CreatePaginationToken(scanResponse.ExclusiveStartKey),
            };

            return response;
        }

        protected async Task<TransactGetItemsResponse> TransactGetItemsAsync<T>(TransactGetItemsRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            return await this.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () => await DynamoClient.TransactGetItemsAsync(request, cancellationToken)).ConfigureAwait(false);
        }

        protected override void DisposeServices()
        {
            context?.Dispose();
            dynamoClient?.Dispose();
        }
    }
}