using Amazon.Lambda.Core;
using Innovt.Cloud.AWS.Lambda.Sqs;
using Innovt.Cloud.Queue;
using Innovt.Core.CrossCutting.Ioc;
using System;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Lambda.Sample;

public class Function : SqsEventProcessor<SampleEvent>
{
    public Function() : base()
    {
    }

    protected override Task ProcessMessage(QueueMessage<SampleEvent> message)
    {
        Console.WriteLine("Stating process message");

        Console.WriteLine($"TraceId Id {message.TraceId}");

        Console.WriteLine($"NUmber of attempts {message.ApproximateReceiveCount}");

        Console.WriteLine($"Body content is: {message.Body}");

        if (message.Body != null && message.Body.Name.Contains("michel"))
            throw new Exception("Testing batch failure");

        foreach (var item in message.Attributes)
            Console.WriteLine($"Attribute Key: {item.Key} and Value is {item.Value}");

        Console.WriteLine("Message process completed");

        return Task.CompletedTask;
    }

    protected override IContainer SetupIocContainer()
    {
        return null;
    }
}