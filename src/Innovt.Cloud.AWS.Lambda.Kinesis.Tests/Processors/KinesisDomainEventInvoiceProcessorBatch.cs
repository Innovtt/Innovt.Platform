// Company: Antecipa
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests
// Solution: Innovt.Platform
// Date: 2021-07-20

using Innovt.Core.CrossCutting.Ioc;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests.Processors
{
    public class KinesisDomainEventInvoiceProcessorBatch : KinesisDomainEventProcessorBatch<InvoiceDomainEvent>
    {
        private readonly IDomainEventServiceMock<InvoiceDomainEvent> serviceMock;

        public KinesisDomainEventInvoiceProcessorBatch(IDomainEventServiceMock<InvoiceDomainEvent> domainEventServiceMock)
        {
            this.serviceMock = domainEventServiceMock ?? throw new System.ArgumentNullException(nameof(domainEventServiceMock));
        }

        protected override Task<BatchFailureResponse> ProcessMessages(IList<InvoiceDomainEvent> messages)
        {
            var result = serviceMock.ProcessMessage(messages.First());

            return Task.FromResult(result);
        }

        protected override IContainer SetupIocContainer()
        {
            serviceMock.InicializeIoc();

            return null;
        }

    }
}