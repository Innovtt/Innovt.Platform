using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Cognito.Tests;

public class BaseIdentityProvider: CognitoIdentityProvider
{
    public BaseIdentityProvider(ILogger logger, IAwsConfiguration configuration, string clientId, string userPoolId, string domainEndPoint, string region = null) : base(logger, configuration, clientId, userPoolId, domainEndPoint, region)
    {
    }
}