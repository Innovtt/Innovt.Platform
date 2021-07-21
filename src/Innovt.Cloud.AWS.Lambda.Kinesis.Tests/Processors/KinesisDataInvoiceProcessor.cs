// Company: Antecipa
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests
// Solution: Innovt.Platform
// Date: 2021-07-20

using System.Threading.Tasks;
using Innovt.Core.CrossCutting.Ioc;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests.Processors
{
    public class KinesisDataInvoiceProcessor : KinesisDataProcessor<Invoice>
    {
        private readonly IServiceMock serviceMock;

        public KinesisDataInvoiceProcessor(IServiceMock serviceMock)
        {
            this.serviceMock = serviceMock ?? throw new System.ArgumentNullException(nameof(serviceMock));
        }

        protected override IContainer SetupIocContainer()
        {
            return null;
        }

        protected override Task ProcessMessage(Invoice message)
        {
            serviceMock.ProcessMessage(message.TraceId);

            return Task.CompletedTask;
        }
    }
}