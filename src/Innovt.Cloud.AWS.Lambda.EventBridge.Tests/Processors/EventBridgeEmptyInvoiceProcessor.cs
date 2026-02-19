// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.EventBridge.Tests

using Amazon.Lambda.CloudWatchEvents;
using Innovt.Core.CrossCutting.Ioc;

namespace Innovt.Cloud.AWS.Lambda.EventBridge.Tests.Processors;

public class EventBridgeEmptyInvoiceProcessor : EventBridgeEventProcessor<EmptyInvoice>
{
    private readonly IServiceMock serviceMock;

    public EventBridgeEmptyInvoiceProcessor(IServiceMock serviceMock)
    {
        this.serviceMock = serviceMock ?? throw new ArgumentNullException(nameof(serviceMock));
    }

    protected override IContainer SetupIocContainer()
    {
        return serviceMock.InitializeIoc();
    }

    protected override Task ProcessMessage(EmptyInvoice message)
    {
        serviceMock.ProcessMessage(message.TraceId);

        return Task.CompletedTask;
    }
}
