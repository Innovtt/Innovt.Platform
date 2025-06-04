using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Dynamo.Tests;

public class DataModelRepository(ILogger logger, IAwsConfiguration configuration)
    : Repository(logger,
        configuration)
{
}