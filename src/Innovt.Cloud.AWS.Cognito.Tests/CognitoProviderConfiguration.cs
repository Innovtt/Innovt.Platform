// Innovt Company
// Author: Michel Borges
// Project: HeyWorld.Auth.Platform

using System;

namespace Innovt.Cloud.AWS.Cognito.Tests;

public class CognitoProviderConfiguration
{
    public CognitoProviderConfiguration(string clientId, string userPoolId, string domainEndPoint, string region)
    {
        ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
        UserPoolId = userPoolId ?? throw new ArgumentNullException(nameof(userPoolId));
        DomainEndPoint = domainEndPoint ?? throw new ArgumentNullException(nameof(domainEndPoint));
        Region = region;
    }


    public CognitoProviderConfiguration()
    {
    }

    public string ClientId { get; set; }
    public string UserPoolId { get; set; }
    public string DomainEndPoint { get; set; }
    public string Region { get; set; }
}