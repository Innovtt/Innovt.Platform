// Company: Antecipa
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests
// Solution: Innovt.Platform
// Date: 2021-07-20

using Innovt.Core.CrossCutting.Ioc;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests.Processors;

public class KinesisDomainEventInvoiceProcessor : KinesisDomainEventProcessor<InvoiceDomainEvent>
{
    private readonly IDomainEventServiceMock<InvoiceDomainEvent> serviceMock;

    public KinesisDomainEventInvoiceProcessor(IDomainEventServiceMock<InvoiceDomainEvent> domainEventServiceMock)
    {
        serviceMock = domainEventServiceMock ?? throw new ArgumentNullException(nameof(domainEventServiceMock));
    }

    protected override IContainer SetupIocContainer()
    {
        serviceMock.InicializeIoc();

        return null;
    }

    protected override Task ProcessMessage(InvoiceDomainEvent message)
    {
        serviceMock.ProcessMessage(message);

        return Task.CompletedTask;
    }
}