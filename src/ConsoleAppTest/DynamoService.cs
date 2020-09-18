using Amazon.DynamoDBv2;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.AWS.Dynamo;
using Innovt.Cloud.Table;
using Innovt.Core.CrossCutting.Log;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleAppTest
{

    public class DynamoService : Repository
    {
        public DynamoService(ILogger logger, IAWSConfiguration configuration) : base(logger, configuration)
        {
        }

        public DynamoService(ILogger logger, IAWSConfiguration configuration, string region) : base(logger, configuration, region)
        {
        }


        public async Task<IList<DynamoTable>> GetAll() {

            //var conditions = new List<ScanCondition>();
            //conditions.Add(new ScanCondition("PartitionKey", ScanOperator.Equal, buyerId));
            //conditions.Add(new ScanCondition("BuyerId", ScanOperator.Equal, buyerId));
            //conditions.Add(new ScanCondition("Type", ScanOperator.Equal, type));
            //var integrationDate = (await base.QueryAsync<TaskIntegrationDate>(conditions, cancellationToken)).FirstOrDefault();
            //return integrationDate;

            var queryOperation = new ScanRequest()
            {
              
            };

            queryOperation.AddCondition("FromAddress", Innovt.Cloud.Table.ComparisonOperator.Equal, "desenvolvimento@antecipa.com")
                .AddCondition("Subject", Innovt.Cloud.Table.ComparisonOperator.Contains, "Subject");

            return await base.ScanAsync<DynamoTable>(queryOperation);
        
        }
    }
}
