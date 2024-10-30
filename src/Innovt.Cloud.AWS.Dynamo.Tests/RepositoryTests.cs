using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.AWS.Dynamo.Tests.Mapping;
using Innovt.Cloud.AWS.Dynamo.Tests.Mapping.Contacts;
using Innovt.Cloud.Table;
using Innovt.Core.CrossCutting.Log;
using NSubstitute;
using NUnit.Framework;
using UserStatus = Innovt.Cloud.AWS.Dynamo.Tests.Mapping.UserStatus;

namespace Innovt.Cloud.AWS.Dynamo.Tests;

[TestFixture]
//[Ignore("Only for local tests")]
public class RepositoryTests
{
    private string fakeUserId = "24a874d8-d0a1-7032-b572-3c3383ff4ba9";
    
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
    }

    private ILogger loggerMock;
    private IAwsConfiguration awsConfigurationMock;
    private SampleRepository repository;

    private async Task<User> AddUserIfNotExist()
    { 
        var userSortKey = "PROFILE";
            
        var queryRequest = new QueryRequest
        {
            KeyConditionExpression = "PK=:pk AND SK=:sk",
            Filter = new
            {
                pk = $"USER#{fakeUserId}",
                sk = userSortKey
            }
        };

        var user = (await repository.QueryAsync<User>(queryRequest).ConfigureAwait(false)).SingleOrDefault();
        
        //Temporary Delete
        if (user is not null)
            await repository.DeleteAsync(user).ConfigureAwait(false);
          //  return user;
        
          
        user = new User
        {
            Id = fakeUserId,
            Email = "michelmob@gmail.com",
            FirstName = "MichelMock",
            LastName = "BorgesMock",
            CreatedAt = DateTime.Now,
            Picture = "https://www.google.com",
            JobPositionId = 1,
            Context = "C2G",
            IsActive = true,
            Status = UserStatus.Active,
            DaysOfWeek = new List<int>(){1,2,3,4,5},
        };
        
        await repository.AddAsync(user).ConfigureAwait(false);

        return user;
    }
    
    [Test]
    public async Task AddDeleteAndQuery()
    {
        var context = new SampleDynamoContext();

        var awsConfiguration = new DefaultAwsConfiguration("c2g-dev");

        repository = new SampleRepository(context, loggerMock, awsConfiguration);
        
        try
        {
            await AddUserIfNotExist().ConfigureAwait(false);
            
            var userSortKey = "PROFILE";
            
            var queryRequest = new QueryRequest
            {
                KeyConditionExpression = "PK=:pk AND SK=:sk",
                Filter = new
                {
                    pk = $"USER#{fakeUserId}",
                    sk = userSortKey
                }
            };

            var user1 = (await repository.QueryAsync<User>(queryRequest).ConfigureAwait(false)).SingleOrDefault();

            var userByIdAndSort =
                await repository.GetByIdAsync<User>($"USER#{fakeUserId}", userSortKey).ConfigureAwait(false);

            Assert.Multiple(() =>
            {
                Assert.That(user1, Is.Not.Null);
                Assert.That(userByIdAndSort, Is.Not.Null);
                Assert.That(user1.Email, Is.EqualTo(userByIdAndSort.Email));
                Assert.That(user1.FirstName, Is.EqualTo(userByIdAndSort.FirstName));
                Assert.That(user1.LastName, Is.EqualTo(userByIdAndSort.LastName));
                Assert.That(user1.Id, Is.EqualTo(userByIdAndSort.Id));
                Assert.That(user1.Status, Is.EqualTo(userByIdAndSort.Status));
                Assert.That(user1.StatusId, Is.EqualTo(userByIdAndSort.StatusId));
                Assert.That(user1.DaysOfWeek, Is.EqualTo(userByIdAndSort.DaysOfWeek));
            });

            if (user1 == null)
            {
                Assert.Fail("User not found");
                return;
            }

            var user2 = user1;
            user2.FirstName = "Michel";
            user2.LastName = "Borges";

            await repository.DeleteAsync(user2).ConfigureAwait(false);

            queryRequest = new QueryRequest
            {
                KeyConditionExpression = "PK=:pk AND SK=:sk",
                Filter = new
                {
                    pk = $"USER#{user2.Id}",
                    sk = userSortKey
                }
            };

            var user3 = (await repository.QueryAsync<User>(queryRequest).ConfigureAwait(false)).SingleOrDefault();

            Assert.That(user3, Is.Null);

            await repository.AddAsync(user2).ConfigureAwait(false);

            user3 = (await repository.QueryAsync<User>(queryRequest).ConfigureAwait(false)).SingleOrDefault();

            Assert.That(user3, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(user2.Email, Is.EqualTo(user3.Email));
                Assert.That(user2.Email, Is.EqualTo(user3.Email));
                Assert.That(user2.FirstName, Is.EqualTo(user3.FirstName));
                Assert.That(user2.FirstName, Is.EqualTo(user3.FirstName));
                Assert.That(user2.LastName, Is.EqualTo(user3.LastName));
                Assert.That(user2.LastName, Is.EqualTo(user3.LastName));
                Assert.That(user2.Id, Is.EqualTo(user3.Id));
                Assert.That(user2.Id, Is.EqualTo(user3.Id));
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [Test]
    public async Task ContextAddAndDeleteBatch()
    {
        var context = new SampleDynamoContext();

        var awsConfiguration = new DefaultAwsConfiguration("c2g-dev");

        repository = new SampleRepository(context, loggerMock, awsConfiguration);

        try
        {
            await AddUserIfNotExist().ConfigureAwait(false);
            
            var userSortKey = "PROFILE";

            var queryRequest = new QueryRequest
            {
                KeyConditionExpression = "PK=:pk AND SK=:sk",
                Filter = new
                {
                    pk = $"USER#{fakeUserId}",
                    sk = userSortKey
                }
            };

            var user = (await repository.QueryAsync<User>(queryRequest).ConfigureAwait(false)).SingleOrDefault();

            var users = new List<User>();

            Assert.That(user, Is.Not.Null);

            for (var i = 0; i < 5; i++)
            {
                var user1 = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = user.Id, //to keep the reference of the user
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CreatedAt = DateTime.Now,
                    Picture = user.Picture,
                    JobPositionId = user.JobPositionId,
                    Context = user.Context,
                    IsActive = user.IsActive
                };
                users.Add(user1);
            }

            await repository.AddRangeAsync(users).ConfigureAwait(false);

            //scan all users
            var scanRequest = new ScanRequest
            {
                FilterExpression = "Email=:email",
                Filter = new
                {
                    email = user.Id
                }
            };

            var usersList = (await repository.ScanAsync<User>(scanRequest).ConfigureAwait(false)).ToList();

            await repository.DeleteRangeAsync(usersList).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [Test]
    public async Task UpdateOperation()
    {
        var context = new SampleDynamoContext();

        var awsConfiguration = new DefaultAwsConfiguration("c2g-dev");

        repository = new SampleRepository(context, loggerMock, awsConfiguration);

        try
        {
            await AddUserIfNotExist().ConfigureAwait(false);
            var userSortKey = "PROFILE";

            var queryRequest = new QueryRequest
            {
                KeyConditionExpression = "PK=:pk AND SK=:sk",
                Filter = new
                {
                    pk = $"USER#{fakeUserId}",
                    sk = userSortKey
                }
            };

            var user =
                (await repository.QueryAsync<User>(queryRequest).ConfigureAwait(false)).SingleOrDefault();

            Assert.That(user, Is.Not.Null);

            user.FirstName = "Rafaela";
            user.LastName = "Borges";

            await repository.UpdateAsync(user).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    [Test]
    public async Task QueryPaginatedBy()
    {
        try
        {
            var context = new SampleDynamoContext();

            var awsConfiguration = new DefaultAwsConfiguration("c2g-dev");

            repository = new SampleRepository(context, loggerMock, awsConfiguration);

            var queryRequest = new QueryRequest
            {
                KeyConditionExpression = "PK=:pk AND begins_with(SK,:sk)",
                FilterExpression = "contains(#Name, :name)",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#Name", "Name" } },
                Filter = new
                {
                    pk = "SKILL",
                    sk = "SKILL#",
                    name = "CLOUD"
                },
                Page = null,
                PageSize = 10
            };

            var skills = await repository.QueryPaginatedByAsync<Skill>(queryRequest).ConfigureAwait(false);

            Assert.That(skills, Is.Not.Null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [Test]
    public async Task TransactionWrite()
    {
        var context = new SampleDynamoContext();

        var awsConfiguration = new DefaultAwsConfiguration("c2g-dev");

        repository = new SampleRepository(context, loggerMock, awsConfiguration);

        var email = "michelmob+innovt@gmail.com";

        var users = await repository.ScanAsync<User>(new ScanRequest
        {
            FilterExpression = "Email=:email",
            Filter = new
            {
                email
            }
        }).ConfigureAwait(false);

        if (users.Any())
        {
            foreach (var userToBeDeleted in users)
            {
                await repository.Delete(userToBeDeleted).ConfigureAwait(false);
            }
        }

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = email,
            FirstName = "Michel",
            LastName = "Borges",
            CreatedAt = DateTime.Now,
            Picture = "https://www.google.com",
            JobPositionId = 1,
            Context = "C2G",
            IsActive = true,
        };

        await repository.SaveUser(user, CancellationToken.None).ConfigureAwait(false);

        Assert.Pass("Transaction Saved");
    }


    [Test]
    public async Task QuerySkill()
    {
        try
        {
            var context = new SampleDynamoContext();

            var awsConfiguration = new DefaultAwsConfiguration("c2g-dev");

            repository = new SampleRepository(context, loggerMock, awsConfiguration);

            var queryRequest = new QueryRequest
            {
                KeyConditionExpression = "PK=:pk AND begins_with(SK,:sk)",
                Filter = new
                {
                    pk = "CE#6f9d96c5-3639-4a78-96d5-50293c30a83e",
                    sk = "CE#SKILL#"
                }
            };

            var expertSkills = await repository.QueryAsync<CloudExpertSkill>(queryRequest, CancellationToken.None)
                .ConfigureAwait(false);

            Assert.That(expertSkills, Is.Not.Null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    [Test]
    public async Task AddAvailability()
    {
        try
        {
            var context = new SampleDynamoContext();

            var awsConfiguration = new DefaultAwsConfiguration("c2g-dev");

            repository = new SampleRepository(context, loggerMock, awsConfiguration);

            var availablity = new Availability();
            availablity.OwnerId = Guid.Parse("6f9d96c5-3639-4a78-96d5-50293c30a83e");
            availablity.TimeZoneId = 3;
            availablity.Days = new List<AvailabilityDay>
            {
                new()
                {
                    StartTime = TimeOnly.MaxValue,
                    AvailableDays = new List<int> { 1, 2 }
                },
                new()
                {
                    StartTime = TimeOnly.MaxValue,
                    AvailableDays = new List<int> { 3, 4 }
                }
            };

            var queryRequest = new QueryRequest
            {
                KeyConditionExpression = "PK=:pk AND SK=:sk",
                Filter = new
                {
                    pk = "CE#6f9d96c5-3639-4a78-96d5-50293c30a83e",
                    sk = "CE#AVAILABILITY"
                }
            };

            var expertSkills = await repository.QueryAsync<Availability>(queryRequest, CancellationToken.None)
                .ConfigureAwait(false);

            Assert.That(expertSkills, Is.Not.Null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [Test]
    public async Task QueryUserWithDateTimeOffSetColumn()
    {
        try
        {
            var context = new SampleDynamoContext();

            var awsConfiguration = new DefaultAwsConfiguration("c2g-dev");

            repository = new SampleRepository(context, loggerMock, awsConfiguration);
            
            var queryRequest = new QueryRequest
            {
                KeyConditionExpression = "PK=:pk AND begins_with(SK,:sk)",
                Filter = new
                {
                    pk = $"USER#34d824f8-a021-7070-e070-e65e7f27107e",
                    sk = "PROFILE"
                }
            };
            
            var expertSkills = await repository.QueryAsync<User>(queryRequest, CancellationToken.None)
                .ConfigureAwait(false);

            Assert.That(expertSkills, Is.Not.Null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
    [Test]
    public async Task AddContactTestingDiscriminator()
    {
        try
        {
            var context = new SampleDynamoContext();

            var awsConfiguration = new DefaultAwsConfiguration("c2g-dev");

            repository = new SampleRepository(context, loggerMock, awsConfiguration);

            var contacts = new List<DynamoContact>();
            
            var phone = new DynamoPhoneContact
            {
                Name = "Michel",
                CountryCode = "55",
                Id = Guid.NewGuid().ToString()
            };
            contacts.Add(phone);
            var email = new DynamoEmailContact
            {
                Name = "Michel",
                Value = "michelmob@gmail.com",
                Days = [1, 2, 3, 4, 5],
                Id = Guid.NewGuid().ToString()
            };
            contacts.Add(email);
            
            //O que vai acontecer eh que ele nao vai cneontrar o tipo pelo nome.
            await repository.AddRangeAsync(contacts).ConfigureAwait(false);
            
            var queryRequest = new QueryRequest
            {
                KeyConditionExpression = "PK=:pk AND begins_with(SK,:sk)",
                Filter = new
                {
                    pk = $"CONTACT",
                    sk = "CONTACT#"
                }
            };
            
            var result = await repository.QueryAsync<DynamoContact>(queryRequest, CancellationToken.None)
                .ConfigureAwait(false);
            
            Assert.That(contacts, Is.Not.Null);

            foreach (var contact in result)
            {
                Assert.That(contact, Is.Not.Null);
                Assert.That(contact.Id, Is.Not.Null);
                
                if (contact is DynamoPhoneContact phoneContact)
                {
                    Assert.That(phoneContact.CountryCode, Is.Not.Null);
                    Assert.That(phoneContact.CountryCode, Is.EqualTo("55"));
                }
                
                if (contact is DynamoEmailContact emailContact)
                {
                    Assert.That(emailContact.Value, Is.Not.Null);
                    Assert.That(emailContact.Value, Is.EqualTo("michelmob@gmail.com"));
                }
            }

            await repository.DeleteRangeAsync(result).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
    [Test]
    public async Task IgnoredPropertiesShouldInvokeMap()
    {
        var context = new SampleDynamoContext();

        var awsConfiguration = new DefaultAwsConfiguration("c2g-dev");

        repository = new SampleRepository(context, loggerMock, awsConfiguration);

        try
        {
            await AddUserIfNotExist().ConfigureAwait(false);
            
            var userSortKey = "PROFILE";

            var queryRequest = new QueryRequest
            {
                KeyConditionExpression = "PK=:pk AND SK=:sk",
                Filter = new
                {
                    pk = $"USER#{fakeUserId}",
                    sk = userSortKey
                }
            };

            var user =
                (await repository.QueryAsync<User>(queryRequest).ConfigureAwait(false)).SingleOrDefault();

            Assert.That(user, Is.Not.Null);
            Assert.That(user.Company, Is.Not.Null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [Test]
    public async Task QueryPaginated()
    {
        var context = new SampleDynamoContext();

        var awsConfiguration = new DefaultAwsConfiguration("c2g-dev");

        repository = new SampleRepository(context, loggerMock, awsConfiguration);

        try
        {
            var queryRequest = new QueryRequest
            {
                KeyConditionExpression = "PK=:pk AND begins_with(SK,:sk)",
                Filter = new
                {
                    pk = "SKILL",
                    sk = "SKILL#",
                    name = "CLOUD"
                },
                PageSize = 10
            };
            
            queryRequest.FilterExpression = "contains(#Name, :name)";
            queryRequest.ExpressionAttributeNames = new Dictionary<string, string> { { "#Name", "Name" } };
        
            var skills = await repository.QueryPaginatedByAsync<Skill>(queryRequest).ConfigureAwait(false);

            Assert.That(skills, Is.Not.Null);

            var scanRequest = new ScanRequest()
            {
                FilterExpression = "contains(#Name, :name)",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#Name", "Name" } },
                Filter = new
                {
                    name = "CLOUD"
                },
                PageSize = 10
            };
            
             skills = await repository.ScanPaginatedByAsync<Skill>(scanRequest).ConfigureAwait(false);
             
             
             Assert.That(skills, Is.Not.Null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    
}