// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Sqs.Tests

using System;
using System.Threading.Tasks;
using Innovt.Cloud.Queue;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda.Sqs.Tests;

public class CustomSqsEventProcessor : SqsEventProcessor<Person>
{
    public CustomSqsEventProcessor(ILogger logger) : base(logger)
    {
    }

    public CustomSqsEventProcessor(bool isFifo, bool reportBatchItemFailures) : base(isFifo, reportBatchItemFailures)
    {
    }

    public CustomSqsEventProcessor()
    {
    }


    protected override IContainer SetupIocContainer()
    {
        return null;
    }

    protected override Task ProcessMessage(QueueMessage<Person> message)
    {
        //fake rule to test some conditions.

        if (message.Body.Name == "michel")
            return Task.CompletedTask;
        throw new Exception("Fake exception");
    }
}