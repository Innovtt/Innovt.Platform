using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Utilities;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Cognito.Tests;

[TestFixture]
[Ignore("Only for local tests")]
public class IntegratedTests
{   
    [SetUp]
    public void TearUp()
    {
        loggerMock = Substitute.For<ILogger>();
        
        var configuration = new ConfigurationBuilder();
        
        var awsOptions = new AWSOptions
        {
            Profile = "hey-dev", Region = RegionEndpoint.SAEast1, DefaultClientConfig =
            {
                AllowAutoRedirect = true
            }
        };
        
        configuration.AddSystemsManager("/Authentication/", awsOptions,false);
        
        awsConfigurationMock = new DefaultAwsConfiguration("hey-dev");
        
        var configurationRoot = configuration.Build();
        
        var cognitoConfiguration = configurationRoot.GetSection("CognitoWebApp").Get<CognitoProviderConfiguration>();

        if (cognitoConfiguration.IsNull())
            cognitoConfiguration = configurationRoot.GetSection("Authentication:CognitoWebApp")
                .Get<CognitoProviderConfiguration>();
        
        identityProvider = new BaseIdentityProvider(
            loggerMock,
            awsConfigurationMock,cognitoConfiguration!.ClientId,
            cognitoConfiguration.UserPoolId,
            cognitoConfiguration.DomainEndPoint,
            cognitoConfiguration.Region);
    }
    
    private ILogger loggerMock;
    
    private IAwsConfiguration awsConfigurationMock;
    private BaseIdentityProvider identityProvider;
    
    //var response = await identityProvider.SignIn(request, cancellationToken);
    
    [Test]
    public async Task TestSignIn()
    {
        
        // Arrange
        var request = new SignInRequest
        {
            UserName = "michel.borges@cloud2gether.com",
            Password = "testpassword"
        };

        try
        {
            var response = await identityProvider.SignIn(request, CancellationToken.None);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        // Act
       
    }
    
      [Test]
        public async Task Register()
        {
            // Arrange
            var request = new SignUpRequest
            {
                UserName = "michel.borges@cloud2gether.com",
                Password = "testpassword"
            };
    
            try
            {
                var response = await identityProvider.SignUp(request, CancellationToken.None);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            // Act
           
        }
}