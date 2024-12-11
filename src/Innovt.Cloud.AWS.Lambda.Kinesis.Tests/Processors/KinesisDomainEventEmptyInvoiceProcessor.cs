﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests

using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Core.Events;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests.Processors;

public class KinesisDomainEventEmptyInvoiceProcessor : KinesisDomainEventProcessor<DomainEvent>
{
    private readonly IDomainEventServiceMock<DomainEvent> serviceMock;

    public KinesisDomainEventEmptyInvoiceProcessor(IDomainEventServiceMock<DomainEvent> domainEventServiceMock)
    {
        serviceMock = domainEventServiceMock ?? throw new ArgumentNullException(nameof(domainEventServiceMock));
    }

    protected override DomainEvent DeserializeBody(string content, string? partition)
    {
        return DomainEvent.Empty(partition);
    }

    protected override IContainer SetupIocContainer()
    {
       return  serviceMock.InicializeIoc();
    }

    protected override Task ProcessMessage(DomainEvent message)
    {
        serviceMock.ProcessMessage(message);

        return Task.CompletedTask;
    }


}