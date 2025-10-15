using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Security;

namespace Innovt.Cloud.AWS.Cognito.Authorization;

[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
public class BasicAuthorizationService(ILogger logger, IAwsConfiguration configuration, string userPoolId)
    : AwsBaseService(logger, configuration), IBasicAuthService
{
    public string UserPoolId { get; } = userPoolId ?? throw new ArgumentNullException(nameof(userPoolId));

    private static readonly ActivitySource BasicAuthorizationServiceActivitySource =
        new(nameof(BasicAuthorizationService));

    private AmazonCognitoIdentityProviderClient cognitoIdentityProvider;

    private AmazonCognitoIdentityProviderClient CognitoProvider
    {
        get { return cognitoIdentityProvider ??= CreateService<AmazonCognitoIdentityProviderClient>(); }
    }

    public async Task<bool> Authenticate(string userName, string password,
        CancellationToken cancellationToken = default)
    {
        using var activity = BasicAuthorizationServiceActivitySource.StartActivity();
        activity?.SetTag("UserName", userName);

        try
        {
            var request = new DescribeUserPoolClientRequest
            {
                UserPoolId = UserPoolId,
                ClientId = userName
            };

            var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await CognitoProvider.DescribeUserPoolClientAsync(request, cancellationToken)
                    .ConfigureAwait(false)).ConfigureAwait(false);

            return response.UserPoolClient.ClientSecret == password;
        }
        catch (ResourceNotFoundException)
        {
            return false;
        }
        catch (UnauthorizedException)
        {
            return false;
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Error on Authenticate user {@UserName}", userName);
            return false;
        }
    }

    protected override void DisposeServices()
    {
        cognitoIdentityProvider?.Dispose();
    }
}