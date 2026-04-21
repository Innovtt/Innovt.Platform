using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Integration;

[SetUpFixture]
public class DynamoLocalFixture
{
    public const string TableName = "ChangeTracking";

    private IContainer? container;

    public static string ServiceUrl { get; private set; } = string.Empty;
    public static IAmazonDynamoDB Client { get; private set; } = null!;
    public static bool Available { get; private set; }

    [OneTimeSetUp]
    public async Task StartAsync()
    {
        if (!await IsDockerAvailableAsync().ConfigureAwait(false))
        {
            Available = false;
            return;
        }

        container = new ContainerBuilder()
            .WithImage("amazon/dynamodb-local:2.5.2")
            .WithPortBinding(8000, true)
            .Build();

        await container.StartAsync().ConfigureAwait(false);

        ServiceUrl = $"http://localhost:{container.GetMappedPublicPort(8000)}";

        var config = new AmazonDynamoDBConfig
        {
            ServiceURL = ServiceUrl,
            AuthenticationRegion = "us-east-1"
        };
        Client = new AmazonDynamoDBClient(new BasicAWSCredentials("test", "test"), config);

        await CreateTableAsync().ConfigureAwait(false);

        Available = true;
    }

    [OneTimeTearDown]
    public async Task StopAsync()
    {
        if (container is not null)
            await container.DisposeAsync().ConfigureAwait(false);

        Client?.Dispose();
    }

    private static async Task<bool> IsDockerAvailableAsync()
    {
        try
        {
            var dockerHost = Environment.GetEnvironmentVariable("DOCKER_HOST");
            if (!string.IsNullOrEmpty(dockerHost))
                return true;

            using var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
            await socket.ConnectAsync(new UnixDomainSocketEndPoint("/var/run/docker.sock"))
                .WaitAsync(TimeSpan.FromSeconds(2)).ConfigureAwait(false);
            return socket.Connected;
        }
        catch
        {
            return false;
        }
    }

    private static async Task CreateTableAsync()
    {
        var deadline = DateTime.UtcNow.AddSeconds(30);
        while (DateTime.UtcNow < deadline)
        {
            try
            {
                await Client.DescribeTableAsync(TableName).ConfigureAwait(false);
                return;
            }
            catch (ResourceNotFoundException)
            {
                break;
            }
            catch
            {
                await Task.Delay(200).ConfigureAwait(false);
            }
        }

        await Client.CreateTableAsync(new CreateTableRequest
        {
            TableName = TableName,
            AttributeDefinitions = new List<AttributeDefinition>
            {
                new("PK", ScalarAttributeType.S),
                new("SK", ScalarAttributeType.S)
            },
            KeySchema = new List<KeySchemaElement>
            {
                new("PK", KeyType.HASH),
                new("SK", KeyType.RANGE)
            },
            BillingMode = BillingMode.PAY_PER_REQUEST
        }).ConfigureAwait(false);
    }
}
