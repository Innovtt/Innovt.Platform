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
using Amazon.DynamoDBv2.DocumentModel;

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
            return base.CreateRetryAsyncPolicy<ProvisionedThroughputExceededException, InternalServerErrorException>();
        }

        public async Task<T> GetByIdAsync<T>(object hashKey, string partitionKey=null, CancellationToken cancellationToken = default) where T : ITableMessage
        {
            var config = new DynamoDBOperationConfig()
            {
                ConsistentRead = true
            };

            var policy = this.CreateDefaultRetryAsyncPolicy();

            if (string.IsNullOrEmpty(partitionKey))
                return await policy.ExecuteAsync(async () => await Context.LoadAsync<T>(hashKey, config, cancellationToken));

            return await policy.ExecuteAsync(async () => await Context.LoadAsync<T>(hashKey, partitionKey, config, cancellationToken));
        }

        public async Task DeleteAsync<T>(T value, CancellationToken cancellationToken = default) where T : ITableMessage
        {
            var policy = this.CreateDefaultRetryAsyncPolicy();

            await policy.ExecuteAsync(async () => await Context.DeleteAsync<T>(value, cancellationToken)).ConfigureAwait(false);
        }

        public async Task DeleteAsync<T>(object id, string partitionKey, CancellationToken cancellationToken = default) where T : ITableMessage
        {
            var policy = this.CreateDefaultRetryAsyncPolicy();

            if (string.IsNullOrEmpty(partitionKey))
                await policy.ExecuteAsync(async () => await Context.DeleteAsync<T>(id, cancellationToken));
            else
                await policy.ExecuteAsync(async () => await Context.DeleteAsync<T>(id, partitionKey, cancellationToken));
        }

        public async Task AddAsync<T>(T message, CancellationToken cancellationToken = default) where T:ITableMessage
        {
            var config = new DynamoDBOperationConfig()
            {
                IgnoreNullValues = true
            };

            var policy = this.CreateDefaultRetryAsyncPolicy();

            await policy.ExecuteAsync(async () => await Context.SaveAsync(message, config, cancellationToken));
        }

        public async Task AddAsync<T>(IList<T> messages, CancellationToken cancellationToken = default) where T : ITableMessage
        {
            if (messages is null)
            {
                throw new System.ArgumentNullException(nameof(messages));
            }

            var config = new DynamoDBOperationConfig()
            {
                IgnoreNullValues = true
            };

            var policy = this.CreateDefaultRetryAsyncPolicy();

            var batch = Context.CreateBatchWrite<T>();

            batch.AddPutItems(messages);

            await policy.ExecuteAsync(async () => await batch.ExecuteAsync(cancellationToken)).ConfigureAwait(false);
        }

        public async Task UpdateAsync(string tableName,Dictionary<string, AttributeValue> key,
            Dictionary<string, AttributeValueUpdate> attributeUpdates,
            CancellationToken cancellationToken = default)
        {  
            var policy = this.CreateDefaultRetryAsyncPolicy();

            await policy.ExecuteAsync(async () => await DynamoClient.UpdateItemAsync(tableName, key, attributeUpdates, cancellationToken)).ConfigureAwait(false);
        }

        public Task<List<T>> FindByAsync<T>(IList<ScanCondition> conditions, CancellationToken cancellationToken = default) where T : ITableMessage
        {
            var config = new DynamoDBOperationConfig()
            {
                ConsistentRead = true, 
            };

            return  CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () => await Context.ScanAsync<T>(conditions, config).GetNextSetAsync(cancellationToken));
        }

        public async Task<IList<T>> QueryAsync<T>(object hashKeyValue, CancellationToken cancellationToken = default) where T : ITableMessage
        {
            var config = new DynamoDBOperationConfig()
            {
               ConsistentRead = true, 
            };

            var result = await Context.QueryAsync<T>(hashKeyValue, config).GetRemainingAsync(cancellationToken);

            return await this.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () => await
                Context.QueryAsync<T>(hashKeyValue,config).GetNextSetAsync(cancellationToken));
        }
        
        // public async Task<IList<T>> QueryAsyncs<T>(object hashKeyValue, CancellationToken cancellationToken = default) where T : ITableMessage
        // {
        //     var config = new QueryOperationConfig()
        //     {
        //         ConsistentRead = true,
        //        KeyExpression = null, 
        //        //Filter = new QueryFilter()
        //        FilterExpression = null,
        //        AttributesToGet = null,
        //        // ConditionalOperator = 
        //        
        //         // PaginationToken = 
        //     };
        //     
        //     //Context.FromScanAsync<T>(new ScanOperationConfig(){})
        //     var config2 = new ScanOperationConfig()
        //     {
        //         FilterExpression = new Expression(),
        //         Filter = new ScanFilter(),
        //         
        //     };
        //
        //             // Context.FromDocuments<T>()
        //         
        //     return await this.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () => await
        //         Context.FromScanAsync<T>(config2).GetNextSetAsync(cancellationToken));
        // }

        protected override void DisposeServices()
        { 
            context?.Dispose();
            dynamoClient?.Dispose();
        }
    }
}