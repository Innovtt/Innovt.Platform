using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.AWS.Dynamo;
using Innovt.Core.CrossCutting.Log;

namespace ConsoleAppTest;

public class DynamoService : Repository
{
    public DynamoService(ILogger logger, IAwsConfiguration configuration) : base(logger, configuration)
    {
    }

    public DynamoService(ILogger logger, IAwsConfiguration configuration, string region) : base(logger, configuration,
        region)
    {
    }
}