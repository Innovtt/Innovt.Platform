using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.Table;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Utilities;
using Polly.Retry;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cloud.AWS.Dynamo
{
    public abstract class TableService<T> : AWSBaseService, ITableService<T> where T : ITableMessage
    {
        public string TableName { get; private set; }

        protected TableService(ILogger logger, string tableName) : base(logger)
        {
            Check.NotNull("message", nameof(tableName));

            this.TableName = tableName;
        }

        protected TableService(IAWSConfiguration configuration, ILogger logger, string tableName) : base(configuration, logger)
        {
            Check.NotNull("message", nameof(tableName));
            this.TableName = tableName;
        }

        protected TableService(IAWSConfiguration configuration, ILogger logger, string tableName, string region) : base(configuration, logger, region)
        {
            Check.NotNull("message", nameof(tableName));
            this.TableName = tableName;
        }

        protected override AsyncRetryPolicy CreateDefaultRetryAsyncPolicy()
        {
            return base.CreateRetryAsyncPolicy<ProvisionedThroughputExceededException, InternalServerErrorException>();
        }

        public async Task<T> GetByIdAsync(string id, string partitionKey=null, CancellationToken cancellationToken = default)
        {
            var config = new DynamoDBOperationConfig()
            {
                OverrideTableName = TableName
            };

            var policy = this.CreateDefaultRetryAsyncPolicy();

            using var dynamoClient = CreateService<AmazonDynamoDBClient>();

            using var context = new DynamoDBContext(dynamoClient);

            if (string.IsNullOrEmpty(partitionKey))
                return await policy.ExecuteAsync(async () => await context.LoadAsync<T>(id, config, cancellationToken));

            return await policy.ExecuteAsync(async () => await context.LoadAsync<T>(id, partitionKey, config, cancellationToken));
        }

        public T GetById(string id, string partitionKey=null)
        {
            return AsyncHelper.RunSync<T>(async () => await GetByIdAsync(id, partitionKey));
        }

        public async Task DeleteAsync(string id, string partitionKey, CancellationToken cancellationToken = default)
        {

            var config = new DynamoDBOperationConfig()
            {
                OverrideTableName = TableName
            };

            var policy = this.CreateDefaultRetryAsyncPolicy();

            using var dynamoClient = CreateService<AmazonDynamoDBClient>();

            using var context = new DynamoDBContext(dynamoClient);

            if (string.IsNullOrEmpty(partitionKey))
                await policy.ExecuteAsync(async () => await context.DeleteAsync<T>(id, config, cancellationToken));
            else
                await policy.ExecuteAsync(async () => await context.DeleteAsync<T>(id, partitionKey, config, cancellationToken));
        }

        public void Delete(string id, string partitionKey)
        {
            AsyncHelper.RunSync(async () => await DeleteAsync(id, partitionKey));
        }

        public async Task AddAsync(T message, CancellationToken cancellationToken = default)
        {
            var config = new DynamoDBOperationConfig()
            {
                OverrideTableName = TableName,
                IgnoreNullValues = true
            };

            var policy = this.CreateDefaultRetryAsyncPolicy();

            using var dynamoClient = CreateService<AmazonDynamoDBClient>();

            using var context = new DynamoDBContext(dynamoClient);

            await policy.ExecuteAsync(async () => await context.SaveAsync(message, config, cancellationToken));
        }

        public void Add(T message)
        {
            AsyncHelper.RunSync(async () => await AddAsync(message));
        }

        public async Task UpdateAsync(Dictionary<string, AttributeValue> key,
            Dictionary<string, AttributeValueUpdate> attributeUpdates,
            CancellationToken cancellationToken = default)
        {  
            var policy = this.CreateDefaultRetryAsyncPolicy();

            using var dynamoClient = CreateService<AmazonDynamoDBClient>();
            
            await policy.ExecuteAsync(async () => await dynamoClient.UpdateItemAsync(TableName, key, attributeUpdates, cancellationToken));
        }

        /// <summary>
        /// Using Default Key for partitionKey and ProvisionedThroughput of 5 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task CreateIfNotExistAsync(CancellationToken cancellationToken = default)
        {
            using var dynamoClient = CreateService<AmazonDynamoDBClient>();

            var tables = await dynamoClient.ListTablesAsync(cancellationToken);

            if (tables.TableNames != null && tables.TableNames.Any(t => t == TableName))
                return;

            var tableRequest = new CreateTableRequest()
            {
                TableName = TableName,
                ProvisionedThroughput = new ProvisionedThroughput(5, 5)
            };

            tableRequest.AttributeDefinitions.Add(new AttributeDefinition("Id", ScalarAttributeType.S));
            tableRequest.AttributeDefinitions.Add(new AttributeDefinition("PartitionKey", ScalarAttributeType.S));
            tableRequest.KeySchema.Add(new KeySchemaElement("Id", KeyType.HASH));
            tableRequest.KeySchema.Add(new KeySchemaElement("PartitionKey", KeyType.RANGE));

            await this.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                    await dynamoClient.CreateTableAsync(tableRequest, cancellationToken));

        }

        public async Task<List<T>> ScanAsync(IList<ScanCondition> conditions, CancellationToken cancellationToken = default)
        {
            var config = new DynamoDBOperationConfig()
            {
                OverrideTableName = TableName,
            };


            using var dynamoClient = CreateService<AmazonDynamoDBClient>();

            using var context = new DynamoDBContext(dynamoClient);

            return await CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () => await context.ScanAsync<T>(conditions, config).GetNextSetAsync(cancellationToken));
        }

        public async Task<List<T>> QueryAsync(object hashKeyValue, CancellationToken cancellationToken = default)
        {
            var config = new DynamoDBOperationConfig()
            {
                OverrideTableName = TableName,
            };

            using var dynamoClient = CreateService<AmazonDynamoDBClient>();

            using var context = new DynamoDBContext(dynamoClient);

            return await this.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () => await
                context.QueryAsync<T>(hashKeyValue, config).GetNextSetAsync(cancellationToken));
        }
    }
}