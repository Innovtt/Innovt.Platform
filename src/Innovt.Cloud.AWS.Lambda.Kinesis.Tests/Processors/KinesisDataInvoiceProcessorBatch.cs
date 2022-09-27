// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests

using Innovt.Core.CrossCutting.Ioc;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests.Processors;

public class KinesisDataInvoiceProcessorBatch : KinesisDataProcessorBatch<Invoice>
{
    private readonly IServiceMock serviceMock;

    public KinesisDataInvoiceProcessorBatch(IServiceMock serviceMock, bool reportItemFailures = false) : base(
        reportItemFailures)
    {
        this.serviceMock = serviceMock ?? throw new ArgumentNullException(nameof(serviceMock));
    }

    protected override IContainer SetupIocContainer()
    {
        serviceMock.InicializeIoc();

        return null;
    }

    protected override Task<BatchFailureResponse> ProcessMessages(IList<Invoice> messages)
    {
        var result = serviceMock.ProcessMessage(messages.First().TraceId);

        return Task.FromResult(result);
    }
}