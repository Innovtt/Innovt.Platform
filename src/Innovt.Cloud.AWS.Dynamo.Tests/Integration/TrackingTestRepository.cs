using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Integration;

internal sealed class TrackingTestRepository(
    ILogger logger,
    IAwsConfiguration configuration,
    string serviceUrl)
    : Repository(logger, configuration)
{
    private readonly string serviceUrl = serviceUrl;

    protected override T CreateService<T>()
    {
        if (typeof(T) == typeof(AmazonDynamoDBClient))
        {
            var config = new AmazonDynamoDBConfig
            {
                ServiceURL = serviceUrl,
                AuthenticationRegion = "us-east-1"
            };
            var credentials = new BasicAWSCredentials("test", "test");
            return (T)(object)new AmazonDynamoDBClient(credentials, config);
        }

        return base.CreateService<T>();
    }
}
