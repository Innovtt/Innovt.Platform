// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests

using Innovt.Core.CrossCutting.Ioc;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests.Processors;

public class KinesisDataInvoiceProcessor : KinesisDataProcessor<Invoice>
{
    private readonly IServiceMock serviceMock;

    public KinesisDataInvoiceProcessor(IServiceMock serviceMock)
    {
        this.serviceMock = serviceMock ?? throw new ArgumentNullException(nameof(serviceMock));
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