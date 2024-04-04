// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Tests

using Amazon.Lambda.Core;
using Innovt.Core.CrossCutting.Log;
using IContainer = Innovt.Core.CrossCutting.Ioc.IContainer;

namespace Innovt.Cloud.AWS.Lambda.Tests;

public class CustomEventProcessor : EventProcessor<Person>
{
    public CustomEventProcessor(ILogger? logger) : base(logger)
    {
    }

    public CustomEventProcessor()
    {
    }

    protected override Task Handle(Person message, ILambdaContext context)
    {
        //for test
        if (message.Name == "Exception")
            throw new Exception("Exception");


        return Task.CompletedTask;
    }

    protected override IContainer SetupIocContainer()
    {
        return null!;
    }
    
}