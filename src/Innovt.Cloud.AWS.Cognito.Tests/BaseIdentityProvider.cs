using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Cognito.Tests;

public class BaseIdentityProvider(
    ILogger logger,
    IAwsConfiguration configuration,
    string clientId,
    string userPoolId,
    string domainEndPoint,
    string? region)
    : CognitoIdentityProvider(logger, configuration, clientId, userPoolId, domainEndPoint, region);