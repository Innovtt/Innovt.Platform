// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.EventBridge.Tests

using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda.EventBridge.Tests.Processors;

public class EventBridgeDataInvoiceProcessor : EventBridgeEventProcessor<Invoice>
{
    private readonly IServiceMock serviceMock;

    public EventBridgeDataInvoiceProcessor(IServiceMock serviceMock)
    {
        this.serviceMock = serviceMock ?? throw new ArgumentNullException(nameof(serviceMock));
    }

    public EventBridgeDataInvoiceProcessor(IServiceMock serviceMock, ILogger logger) : base(logger)
    {
        this.serviceMock = serviceMock ?? throw new ArgumentNullException(nameof(serviceMock));
    }

    protected override IContainer SetupIocContainer()
    {
        return serviceMock.InitializeIoc();
    }

    protected override Task ProcessMessage(Invoice message)
    {
        serviceMock.ProcessMessage(message.TraceId);

        return Task.CompletedTask;
    }

    public ILogger GetLogger()
    {
        return Logger;
    }
}
