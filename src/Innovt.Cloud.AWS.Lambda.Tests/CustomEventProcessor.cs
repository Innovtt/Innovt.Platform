// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Tests

using System.ComponentModel;
using Amazon.Lambda.Core;
using Innovt.Core.CrossCutting.Log;
using Microsoft.Extensions.Configuration;
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
        return null;
    }

    protected override void EnrichConfiguration(ConfigurationBuilder configurationBuilder)
    {
        base.EnrichConfiguration(configurationBuilder);
    }
}