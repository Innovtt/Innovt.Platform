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



    }
}
