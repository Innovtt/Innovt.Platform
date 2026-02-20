// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests

using System.Diagnostics.CodeAnalysis;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Domain.Core.Events;

namespace Innovt.Cloud.AWS.Lambda.EventBridge.Tests.Processors;

public class EventBridgeDomainEventEmptyInvoiceProcessor(IDomainEventServiceMock<DomainEvent> domainEventServiceMock)
    : EventBridgeDomainEventProcessor<DomainEvent>
{
    private readonly IDomainEventServiceMock<DomainEvent> serviceMock = domainEventServiceMock ?? throw new ArgumentNullException(nameof(domainEventServiceMock));

    protected override DomainEvent DeserializeBody(string content, string? partition=null)
    {
        return DomainEvent.Empty(partition);
    }

    protected override IContainer SetupIocContainer()
    {
        return serviceMock.InicializeIoc();
    }

    protected override Task ProcessMessage([NotNull]DomainEvent message)
    {
        serviceMock.ProcessMessage(message);

        return Task.CompletedTask;
    }
}