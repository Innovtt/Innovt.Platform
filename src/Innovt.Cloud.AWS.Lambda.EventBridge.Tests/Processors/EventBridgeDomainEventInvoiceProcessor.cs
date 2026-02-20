// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.EventBridge.Tests

using System.Diagnostics.CodeAnalysis;
using Innovt.Core.CrossCutting.Ioc;

namespace Innovt.Cloud.AWS.Lambda.EventBridge.Tests.Processors;

public class EventBridgeDomainEventInvoiceProcessor(IDomainEventServiceMock<InvoiceDomainEvent> domainEventServiceMock)
    : EventBridgeDomainEventProcessor<InvoiceDomainEvent>
{
    private readonly IDomainEventServiceMock<InvoiceDomainEvent> serviceMock = domainEventServiceMock ?? throw new ArgumentNullException(nameof(domainEventServiceMock));

    protected override IContainer SetupIocContainer()
    {
        serviceMock.InicializeIoc();

        return null!;
    }

    protected override Task ProcessMessage([NotNull]InvoiceDomainEvent message)
    {
        serviceMock.ProcessMessage(message);

        return Task.CompletedTask;
    }
}
