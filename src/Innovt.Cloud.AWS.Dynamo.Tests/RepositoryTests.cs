using System;
using System.Linq;
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
            var userId = "74386478-c0d1-70cf-76be-f9d17febd525";
            var userSortKey = "PROFILE";
            
            var queryRequest = new QueryRequest
            {
                KeyConditionExpression = "PK=:pk AND SK=:sk",
                Filter = new
                {
                    pk =  $"USER#{userId}",
                    sk =  userSortKey
                }
            };
            
            var user1 = (await repository.QueryAsync<User>(queryRequest, default).ConfigureAwait(false)).SingleOrDefault();
            
            var userById = await repository.GetByIdAsync<User>(userId).ConfigureAwait(false);
            
            var userByIdAndSort = await repository.GetByIdAsync<User>(userId, userSortKey).ConfigureAwait(false);
            
            Assert.Multiple(() =>
            {
                Assert.That(user1, Is.Not.Null);
                Assert.That(userById, Is.Not.Null);
                Assert.That(userByIdAndSort, Is.Not.Null);
            });
        
            Assert.That(user1.Email, Is.EqualTo(userById.Email));
            Assert.That(user1.Email, Is.EqualTo(userByIdAndSort.Email));
            Assert.That(user1.FirstName, Is.EqualTo(userById.FirstName));
            Assert.That(user1.FirstName, Is.EqualTo(userByIdAndSort.FirstName));
            Assert.That(user1.LastName, Is.EqualTo(userById.LastName));
            Assert.That(user1.LastName, Is.EqualTo(userByIdAndSort.LastName));
            Assert.That(user1.Id, Is.EqualTo(userById.Id));
            Assert.That(user1.Id, Is.EqualTo(userByIdAndSort.Id));

            var user2 = user1;

            user2.Id = "59c6be94-eeea-4185-ab59-fc66207cf387";

            await repository.AddAsync<User>(user2, default).ConfigureAwait(false);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    } 
}