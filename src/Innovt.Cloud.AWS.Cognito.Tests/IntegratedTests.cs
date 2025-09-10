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
//[Ignore("Only for local tests")]
public class IntegratedTests
{   
    [SetUp]
    public void TearUp()
    {
        loggerMock = Substitute.For<ILogger>();
        
        var configuration = new ConfigurationBuilder();
        
        var awsOptions = new AWSOptions
        {
            Profile = "c2g-dev", Region = RegionEndpoint.USEast1, DefaultClientConfig =
            {
                AllowAutoRedirect = true
            }
        };
        
        configuration.AddSystemsManager("/Authentication/", awsOptions,false);
        
        awsConfigurationMock = new DefaultAwsConfiguration("c2g-dev");
        
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
    public async Task LinkUsers()
    {
        // Arrange
        var request = new LinkSocialAccountRequest()
        {
            UserName = "LinkedIn_uA_qHMDw1j",
            Email = "davi262016@gmail.com"
        };

        try
        {
            var response = await identityProvider.LinkSocialUser(request, CancellationToken.None);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
    [Test]
    public async Task Register()
    {
        // Arrange
        var request = new SignUpRequest
        {
            UserName = "michelmob@gmail.com",
            Password = "34567890",
            Family_Name = "michel",
            Name = "Michel",
            Phone_Number = "+5511999999999",
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
    }
    
    [Test]
    public async Task ClearSocialAccounts()
    {
        // Arrange
        var request = new ClearSocialAccountRequest("michelmob@gmail.com");

        try
        {
            var response = await identityProvider.ClearSocialAccounts(request, CancellationToken.None);
            
            Assert.That(response, Is.True);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Assert.Fail(e.Message);
        }
    }

}