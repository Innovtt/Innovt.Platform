using System;
using System.Threading.Tasks;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.AWS.Dynamo.Tests.Mapping;
using Innovt.Cloud.Table;
using Innovt.Core.CrossCutting.Log;
using NSubstitute;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Dynamo.Tests;

[TestFixture]
public class RepositoryTests
{
    private ILogger loggerMock;
    private IAwsConfiguration awsConfigurationMock;
    private SampleRepository repository;

    [SetUp]
    public void TearUp()
    {
        loggerMock = Substitute.For<ILogger>();
        awsConfigurationMock = Substitute.For<IAwsConfiguration>();
        repository = new SampleRepository(new SampleDynamoContext(), loggerMock, awsConfigurationMock);
    }
    
    
    [TearDown]
    public void TearDown()
    {
        loggerMock = null;
        awsConfigurationMock = null;
        repository.Dispose();
        repository = null;
    }
    
    [Test]
    public async Task GetByIdAsync()
    {
        var context = new SampleDynamoContext();
        
        var awsConfiguration = new DefaultAWSConfiguration("c2g-dev");
        
        repository = new SampleRepository(context, loggerMock, awsConfiguration);
        try
        {   
            var queryRequest = new QueryRequest
            {
                KeyConditionExpression = "PK=:pk AND SK=:sk",
                Filter = new
                {
                    pk =  "USER#74386478-c0d1-70cf-76be-f9d17febd525",
                    sk =  "PROFILE"
                }
            };
            
            var result = await repository.QueryAsync<User>(queryRequest, default).ConfigureAwait(false);
            
            Assert.That(result, Is.Not.Null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        
        



    } 
}