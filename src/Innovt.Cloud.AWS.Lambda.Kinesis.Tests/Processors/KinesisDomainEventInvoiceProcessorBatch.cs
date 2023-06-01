// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests

using Innovt.Core.CrossCutting.Ioc;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests.Processors;

public class KinesisDomainEventInvoiceProcessorBatch : KinesisDomainEventProcessorBatch<InvoiceDomainEvent>
{
    private readonly IDomainEventServiceMock<InvoiceDomainEvent> serviceMock;

    public KinesisDomainEventInvoiceProcessorBatch(IDomainEventServiceMock<InvoiceDomainEvent> domainEventServiceMock)
    {
        serviceMock = domainEventServiceMock ?? throw new ArgumentNullException(nameof(domainEventServiceMock));
    }

    protected override Task<BatchFailureResponse> ProcessMessages(IList<InvoiceDomainEvent> messages)
    {
        var result = serviceMock.ProcessMessage(messages.First());

        return Task.FromResult(result);
    }

    protected override IContainer SetupIocContainer()
    {
        serviceMock.InicializeIoc();

        return null!;
    }
}