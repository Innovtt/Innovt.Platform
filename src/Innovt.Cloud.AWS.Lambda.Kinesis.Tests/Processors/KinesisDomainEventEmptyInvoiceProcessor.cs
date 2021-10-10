// Company: Antecipa
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests
// Solution: Innovt.Platform
// Date: 2021-07-20

using System.Threading.Tasks;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Domain.Core.Events;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests.Processors
{
    public class KinesisDomainEventEmptyInvoiceProcessor : KinesisDomainEventProcessor<DomainEvent>
    {
        private readonly IDomainEventServiceMock<DomainEvent> serviceMock;

        public KinesisDomainEventEmptyInvoiceProcessor(IDomainEventServiceMock<DomainEvent> domainEventServiceMock)
        {
            this.serviceMock = domainEventServiceMock ?? throw new System.ArgumentNullException(nameof(domainEventServiceMock));
        }
        
        protected override DomainEvent DeserializeBody(string content, string partition)
        {
            return DomainEvent.Empty(partition);
        }
        protected override IContainer SetupIocContainer()
        {   
            serviceMock.InicializeIoc();
            return null;
        }
        protected override Task ProcessMessage(DomainEvent message)
        {
            serviceMock.ProcessMessage(message);

            return Task.CompletedTask;
        }
    }
}