// Company: Antecipa
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests
// Solution: Innovt.Platform
// Date: 2021-07-20

using System.Threading.Tasks;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Domain.Core.Streams;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests.Processors
{
    public class KinesisDataInvoiceProcessor2 : KinesisDataProcessor<DataStream<BaseInvoice>>
    {
        private readonly IDataServiceMock<BaseInvoice> serviceMock;

        public KinesisDataInvoiceProcessor2(IDataServiceMock<BaseInvoice> serviceMock)
        {
            this.serviceMock = serviceMock ?? throw new System.ArgumentNullException(nameof(serviceMock));
        }

        protected override IContainer SetupIocContainer()
        {
            serviceMock.InicializeIoc();
            return null;
        }
        
        protected override Task ProcessMessage(DataStream<BaseInvoice> message)
        {   
             serviceMock.ProcessMessage(message);

             return Task.CompletedTask;
        }
    }
}