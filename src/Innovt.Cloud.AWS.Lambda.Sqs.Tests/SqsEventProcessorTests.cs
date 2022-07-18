using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.Lambda.TestUtilities;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Innovt.Cloud.AWS.Lambda.Sqs.Tests;

public class Tests
{
    private TestLambdaContext lambdaContext;

    [SetUp]
    public void Setup()
    {
        lambdaContext = new TestLambdaContext
        {
            Logger = Substitute.For<ILambdaLogger>()
        };
    }

    [Test]
    public async Task Handle_BatchMessage_Returns_Empty_When_ThereIsNoMessages()
    {
        var processor = new CustomSqsEventProcessor();

        var result = await processor.Process(new SQSEvent(), lambdaContext);

        Assert.IsNotNull(result);
        Assert.IsNull(result.BatchItemFailures);
    }


    [Test]
    public async Task Handle_BatchMessage_ForFifoQueue_Returns_TheRemainingMessages_WhenFindTheFirstFailure()
    {
        var processor = new CustomSqsEventProcessor(true, true);

        var messages = new List<SQSEvent.SQSMessage>();

        var expectedMessageIdFailed = new List<string>
        {
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()
        };

        messages.Add(new SQSEvent.SQSMessage
        {
            Body = JsonSerializer.Serialize(new Person("michel")),
            MessageId = Guid.NewGuid().ToString()
        });
        ;

        messages.Add(new SQSEvent.SQSMessage
        {
            Body = JsonSerializer.Serialize(new Person("magal")),
            MessageId = expectedMessageIdFailed[0]
        });

        messages.Add(new SQSEvent.SQSMessage
        {
            Body = JsonSerializer.Serialize(new Person("ti")),
            MessageId = expectedMessageIdFailed[1]
        });

        messages.Add(new SQSEvent.SQSMessage
        {
            Body = JsonSerializer.Serialize(new Person("ale")),
            MessageId = expectedMessageIdFailed[2]
        });

        var sQSEvent = new SQSEvent
        {
            Records = messages
        };

        //Will fail when person name is not michel

        var result = await processor.Process(sQSEvent, lambdaContext);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.BatchItemFailures);
        Assert.AreEqual(result.BatchItemFailures.Count, expectedMessageIdFailed.Count);

        foreach (var item in result.BatchItemFailures)
            if (!expectedMessageIdFailed.Contains(item.ItemIdentifier))
                Assert.Fail(item.ItemIdentifier);
    }


    [Test]
    public async Task Handle_BatchMessage_Returns_FailedMessages_WhenQueueIsStandard()
    {
        var processor = new CustomSqsEventProcessor(false, true);

        var messages = new List<SQSEvent.SQSMessage>();

        var expectedMessageIdFailed = new List<string>
        {
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()
        };

        messages.Add(new SQSEvent.SQSMessage
        {
            Body = JsonSerializer.Serialize(new Person("magal")),
            MessageId = expectedMessageIdFailed[0]
        });

        messages.Add(new SQSEvent.SQSMessage
        {
            Body = JsonSerializer.Serialize(new Person("michel")),
            MessageId = Guid.NewGuid().ToString()
        });

        messages.Add(new SQSEvent.SQSMessage
        {
            Body = JsonSerializer.Serialize(new Person("ale")),
            MessageId = expectedMessageIdFailed[1]
        });

        messages.Add(new SQSEvent.SQSMessage
        {
            Body = JsonSerializer.Serialize(new Person("michel")),
            MessageId = Guid.NewGuid().ToString()
        });

        var sQSEvent = new SQSEvent
        {
            Records = messages
        };

        //Will fail when person name is not michel
        var result = await processor.Process(sQSEvent, lambdaContext);

        Assert.IsNotNull(result);
        Assert.IsNotNull(result.BatchItemFailures);
        Assert.AreEqual(result.BatchItemFailures.Count, expectedMessageIdFailed.Count);
    }

    [Test]
    public void Handle_BatchMessage_Returns_Exception_WhenFunctionDoesNotReportItemFailuers()
    {
        var processor = new CustomSqsEventProcessor(false, false);

        var messages = new List<SQSEvent.SQSMessage>();

        var expectedMessageIdFailed = new List<string>
        {
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()
        };

        messages.Add(new SQSEvent.SQSMessage
        {
            Body = JsonSerializer.Serialize(new Person("magal")),
            MessageId = expectedMessageIdFailed[0]
        });

        messages.Add(new SQSEvent.SQSMessage
        {
            Body = JsonSerializer.Serialize(new Person("michel")),
            MessageId = Guid.NewGuid().ToString()
        });

        messages.Add(new SQSEvent.SQSMessage
        {
            Body = JsonSerializer.Serialize(new Person("ale")),
            MessageId = expectedMessageIdFailed[1]
        });

        messages.Add(new SQSEvent.SQSMessage
        {
            Body = JsonSerializer.Serialize(new Person("michel")),
            MessageId = Guid.NewGuid().ToString()
        });

        var sQSEvent = new SQSEvent
        {
            Records = messages
        };

        //Will fail when person name is not michel
        Assert.ThrowsAsync<Exception>(async () => await processor.Process(sQSEvent, lambdaContext), "Fake exception");
    }

    [Test]
    public async Task Handle_BatchMessage_Returns_EmptyBatchResponseWhen_ThereIsNoFailures()
    {
        var processor = new CustomSqsEventProcessor(false, true);

        var messages = new List<SQSEvent.SQSMessage>
        {
            new()
            {
                Body = JsonSerializer.Serialize(new Person("michel")),
                MessageId = Guid.NewGuid().ToString()
            },

            new()
            {
                Body = JsonSerializer.Serialize(new Person("michel")),
                MessageId = Guid.NewGuid().ToString()
            }
        };

        var sQSEvent = new SQSEvent
        {
            Records = messages
        };

        //Will fail when person name is not michel
        var result = await processor.Process(sQSEvent, lambdaContext);

        Assert.IsNotNull(result);
        Assert.IsNull(result.BatchItemFailures);
    }
}