﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests

using Innovt.Core.CrossCutting.Ioc;

namespace Innovt.Cloud.AWS.Lambda.Kinesis.Tests.Processors;

public class KinesisDataInvoiceProcessorBatch(IServiceMock serviceMock, bool reportItemFailures = false)
    : KinesisDataProcessorBatch<Invoice>(reportItemFailures)
{
    private readonly IServiceMock serviceMock = serviceMock ?? throw new ArgumentNullException(nameof(serviceMock));

    protected override IContainer SetupIocContainer()
    {
        serviceMock.InitializeIoc();

        return default!;
    }

    protected override Task<BatchFailureResponse> ProcessMessages(IList<Invoice> messages)
    {
        var result = serviceMock.ProcessMessage(messages.First().TraceId);

        return Task.FromResult(result);
    }
}