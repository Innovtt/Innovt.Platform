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

namespace Innovt.Cloud.AWS.Dynamo
{
    public abstract class Repository : AwsBaseService, ITableRepository
    {
        private DynamoDBContext context = null;
        private AmazonDynamoDBClient dynamoClient = null;

        protected Repository(ILogger logger,IAWSConfiguration configuration, string region) : base(logger,configuration, region)
        {
            dynamoClient = CreateService<AmazonDynamoDBClient>();

            context = new DynamoDBContext(dynamoClient);
        }

        protected Repository(ILogger logger) : this(logger,null, null)
        {
           
        }

        protected Repository( ILogger logger,IAWSConfiguration configuration) : this(logger,configuration,null)
        {
        }

        protected override AsyncRetryPolicy CreateDefaultRetryAsyncPolicy()
        {
            return base.CreateRetryAsyncPolicy<ProvisionedThroughputExceededException, InternalServerErrorException>();
        }

        public async Task<T> GetByIdAsync<T>(object hashKey, string partitionKey=null, CancellationToken cancellationToken = default)
        {
            var config = new DynamoDBOperationConfig()
            {
                ConsistentRead = true
            };

            var policy = this.CreateDefaultRetryAsyncPolicy();
        
            if (string.IsNullOrEmpty(partitionKey))
                return await policy.ExecuteAsync(async () => await context.LoadAsync<T>(hashKey, config, cancellationToken));

            return await policy.ExecuteAsync(async () => await context.LoadAsync<T>(hashKey, partitionKey, config, cancellationToken));
        }

        public async Task DeleteAsync<T>(T value, CancellationToken cancellationToken = default)
        {
            var policy = this.CreateDefaultRetryAsyncPolicy();

            await policy.ExecuteAsync(async () => await context.DeleteAsync<T>(value, cancellationToken)).ConfigureAwait(false);
        }

        public async Task DeleteAsync<T>(object id, string partitionKey, CancellationToken cancellationToken = default)
        {
            var policy = this.CreateDefaultRetryAsyncPolicy();

            if (string.IsNullOrEmpty(partitionKey))
                await policy.ExecuteAsync(async () => await context.DeleteAsync<T>(id, cancellationToken));
            else
                await policy.ExecuteAsync(async () => await context.DeleteAsync<T>(id, partitionKey, cancellationToken));
        }

        public async Task AddAsync<T>(T message, CancellationToken cancellationToken = default) where T:ITableMessage
        {
            var config = new DynamoDBOperationConfig()
            {
                IgnoreNullValues = true
            };

            var policy = this.CreateDefaultRetryAsyncPolicy();

            await policy.ExecuteAsync(async () => await context.SaveAsync(message, config, cancellationToken));
        }

        public async Task AddAsync<T>(IList<T> messages, CancellationToken cancellationToken = default) where T : ITableMessage
        {
            if (messages is null)
            {
                throw new System.ArgumentNullException(nameof(messages));
            }

            var config = new DynamoDBOperationConfig()
            {
                IgnoreNullValues = true, 
            };

            var policy = this.CreateDefaultRetryAsyncPolicy();

            var batch = context.CreateBatchWrite<T>();

            batch.AddPutItems(messages);

            await policy.ExecuteAsync(async () => await batch.ExecuteAsync(cancellationToken)).ConfigureAwait(false);
        }

        public async Task UpdateAsync(string tableName,Dictionary<string, AttributeValue> key,
            Dictionary<string, AttributeValueUpdate> attributeUpdates,
            CancellationToken cancellationToken = default)
        {  
            var policy = this.CreateDefaultRetryAsyncPolicy();

            await policy.ExecuteAsync(async () => await dynamoClient.UpdateItemAsync(tableName, key, attributeUpdates, cancellationToken)).ConfigureAwait(false);
        }

        public async Task<List<T>> FindByAsync<T>(IList<ScanCondition> conditions, CancellationToken cancellationToken = default)
        {
            var config = new DynamoDBOperationConfig()
            {
                ConsistentRead = true, 
            };

            return await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () => await context.ScanAsync<T>(conditions, config).GetNextSetAsync(cancellationToken));
        }

        public async Task<IList<T>> QueryAsync<T>(object hashKeyValue, CancellationToken cancellationToken = default)
        {
            var config = new DynamoDBOperationConfig()
            {
               ConsistentRead = true
            };

            return await this.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () => await
                context.QueryAsync<T>(hashKeyValue,config).GetNextSetAsync(cancellationToken));
        }

        //public async Task<IList<T>> QueryAsync<T>(object hashKeyValue, CancellationToken cancellationToken = default)
        //{
        //    var config = new DynamoDBOperationConfig()
        //    {
        //        ConsistentRead = true,
        //    };

        //    return await this.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () => await
        //        context.QueryAsync<T>(hashKeyValue, config).GetNextSetAsync(cancellationToken));
        //}
    }

  
}