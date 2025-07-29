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
            // Act
           
        }
        
        [Test]
        public async Task LinkSocialUsers()
        {
            // Arrange
            var request = new LinkSocialAccountRequest()
            {
                Email = "michelmob@gmail.com",
                UserName = "Google_110966145042332068325"
            };
            
            try
            {
                // e se aqui no link eu verificar se o usuário já existe e deletar o usuário antigo ? 
                //Pego o user que está mergedando e deleto o user que esta sendo linkado.
                
                var response = await identityProvider.LinkSocialUser(request, CancellationToken.None);
                
                //e deletar o anterirr.
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            // Act
           
        }
}