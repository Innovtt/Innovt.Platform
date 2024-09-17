using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Dynamo.Tests;

public class SampleRepository : Repository
{
    public SampleRepository(DynamoContext dynamoContext, ILogger logger, IAwsConfiguration configuration) : base(logger, configuration,dynamoContext)
    {
    }
}