// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests

using Innovt.Core.CrossCutting.Ioc;
using Innovt.Domain.Core.Streams;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests.Processors;

public class KinesisDataInvoiceProcessor2 : KinesisDataProcessor<DataStream<BaseInvoice>>
{
    private readonly IDataServiceMock<BaseInvoice> serviceMock;

    public KinesisDataInvoiceProcessor2(IDataServiceMock<BaseInvoice> serviceMock)
    {
        this.serviceMock = serviceMock ?? throw new ArgumentNullException(nameof(serviceMock));
    }

    protected override IContainer SetupIocContainer()
    {
        serviceMock.InicializeIoc();
        return null!;
    }

    protected override Task ProcessMessage(DataStream<BaseInvoice> message)
    {
        serviceMock.ProcessMessage(message);

        return Task.CompletedTask;
    }
}