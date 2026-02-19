// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.EventBridge.Tests

using System.Diagnostics;
using Amazon.Lambda.CloudWatchEvents;
using Amazon.Lambda.TestUtilities;
using Innovt.Cloud.AWS.Lambda.EventBridge.Tests.Processors;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Core.CrossCutting.Log;
using Innovt.CrossCutting.Log.Serilog;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Innovt.Cloud.AWS.Lambda.EventBridge.Tests;

[TestFixture]
public class EventBridgeProcessorTests
{
    [SetUp]
    public void Setup()
    {
        serviceMock = Substitute.For<IServiceMock>();
    }

    private IServiceMock serviceMock = null!;

    private static readonly ActivitySource EventBridgeProcessorTestsActivitySource =
        new("Innovt.Cloud.AWS.Lambda.EventBridge.Tests");

    [Test]
    public void ProcessThrowExceptionIfMessageIsNull()
    {
        var function = new EventBridgeDataInvoiceProcessor(serviceMock);

        Assert.ThrowsAsync<ArgumentNullException>(async () => await function.Process(null!, null!));
    }

    [Test]
    public void ProcessThrowExceptionIfContextIsNull()
    {
        var function = new EventBridgeDataInvoiceProcessor(serviceMock);

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await function.Process(new CloudWatchEvent<Invoice>(), null!));
    }

    [Test]
    public async Task ProcessShouldCallIoc()
    {
        var function = new EventBridgeDataInvoiceProcessor(serviceMock);

        var lambdaContext = new TestLambdaContext();

        var message = CreateValidCloudWatchEvent(new Invoice { NetValue = 100 });

        await function.Process(message, lambdaContext);

        serviceMock.Received().InitializeIoc();
        serviceMock.Received().ProcessMessage(Arg.Any<string>());
    }

    [Test]
    public async Task ProcessWithNullDetailShouldNotCallProcessMessage()
    {
        var function = new EventBridgeDataInvoiceProcessor(serviceMock);

        var lambdaContext = new TestLambdaContext();

        var message = new CloudWatchEvent<Invoice>
        {
            Id = Guid.NewGuid().ToString(),
            Source = "test.source",
            DetailType = "TestDetail",
            Region = "us-east-1",
            Time = DateTime.UtcNow,
            Detail = null!
        };

        await function.Process(message, lambdaContext);

        serviceMock.Received().InitializeIoc();
        serviceMock.DidNotReceive().ProcessMessage(Arg.Any<string>());
    }

    [Test]
    public async Task Process()
    {
        var function = new EventBridgeDataInvoiceProcessor(serviceMock);

        var lambdaContext = new TestLambdaContext();

        var eventId = Guid.NewGuid().ToString();

        var message = CreateValidCloudWatchEvent(new Invoice { NetValue = 50 }, eventId);

        await function.Process(message, lambdaContext);

        serviceMock.Received().InitializeIoc();
        serviceMock.Received().ProcessMessage(Arg.Any<string>());
    }

    [Test]
    public async Task Process_WithoutCustomLogger()
    {
        var emptyContainer = new Innovt.CrossCutting.IOC.Container();

        serviceMock.InitializeIoc().Returns(emptyContainer);

        var function = new EventBridgeDataInvoiceProcessor(serviceMock);

        var lambdaContext = new TestLambdaContext();

        var message = CreateValidCloudWatchEvent(new Invoice { NetValue = 50 });

        await function.Process(message, lambdaContext);

        var logger = function.GetLogger();

        Assert.That(logger, Is.Not.Null);
        Assert.That(logger, Is.InstanceOf<LambdaLogger>());

        serviceMock.Received().InitializeIoc();
        serviceMock.Received().ProcessMessage(Arg.Any<string>());
    }

    [Test]
    public async Task Process_WithCustomLogger()
    {
        var container = new Innovt.CrossCutting.IOC.Container();

        var module = new IocModule();
        module.GetServices().AddScoped<ILogger, Logger>();

        container.AddModule(module);

        serviceMock.InitializeIoc().Returns(container);

        var function = new EventBridgeDataInvoiceProcessor(serviceMock);

        var lambdaContext = new TestLambdaContext();

        var message = CreateValidCloudWatchEvent(new Invoice { NetValue = 50 });

        await function.Process(message, lambdaContext);

        var logger = function.GetLogger();

        Assert.That(logger, Is.Not.Null);
        Assert.That(logger, Is.Not.InstanceOf<LambdaLogger>());
        Assert.That(logger, Is.InstanceOf<Logger>());

        serviceMock.Received().InitializeIoc();
        serviceMock.Received().ProcessMessage(Arg.Any<string>());
    }

    [Test]
    public async Task ProcessShouldSetTraceIdWhenBodyHasNoTraceId()
    {
        var function = new EventBridgeDataInvoiceProcessor(serviceMock);

        var lambdaContext = new TestLambdaContext();

        var message = CreateValidCloudWatchEvent(new Invoice { NetValue = 50 });

        //checking activity
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault().AddService("Innovt.Cloud.AWS.Lambda.EventBridge.Tests"))
            .AddSource("Innovt.Cloud.AWS.Lambda.EventProcessor")
            .AddSource("Innovt.Cloud.AWS.Lambda.EventBridge.Tests")
            .Build();
        var activityName = "Test";
        using var activity = EventBridgeProcessorTestsActivitySource.StartActivity(activityName);
        var rootId = activity?.RootId;

        await function.Process(message, lambdaContext);

        serviceMock.Received().InitializeIoc();
        Assert.That(rootId, Is.Not.Null);
        serviceMock.Received().ProcessMessage(Arg.Is<string>(p => p!.Contains(rootId!)));
    }

    [Test]
    public async Task ProcessShouldMapEnvelopeFieldsToDataStream()
    {
        var function = new EventBridgeDataInvoiceProcessor(serviceMock);

        var lambdaContext = new TestLambdaContext();

        var eventId = Guid.NewGuid().ToString();
        var source = "my.custom.source";
        var eventTime = DateTime.UtcNow;

        var invoice = new Invoice { NetValue = 75 };

        var message = new CloudWatchEvent<Invoice>
        {
            Id = eventId,
            Source = source,
            DetailType = "InvoiceCreated",
            Region = "us-east-1",
            Time = eventTime,
            Detail = invoice
        };

        await function.Process(message, lambdaContext);

        Assert.That(invoice.EventId, Is.EqualTo(eventId));
        Assert.That(invoice.Partition, Is.EqualTo(source));
        Assert.That(invoice.ApproximateArrivalTimestamp, Is.EqualTo(eventTime));

        serviceMock.Received().ProcessMessage(Arg.Any<string>());
    }

    [Test]
    public async Task ProcessDomainEvent()
    {
        var domainMock = Substitute.For<IDomainEventServiceMock<InvoiceDomainEvent>>();

        var function = new EventBridgeDomainEventInvoiceProcessor(domainMock);

        var lambdaContext = new TestLambdaContext();

        var netValue = 10;

        var message = CreateValidCloudWatchEvent(new InvoiceDomainEvent { NetValue = netValue });

        await function.Process(message, lambdaContext);

        domainMock.Received().InicializeIoc();
        domainMock.Received()
            .ProcessMessage(Arg.Is<InvoiceDomainEvent>(p => p.NetValue == netValue));
    }

    [Test]
    public async Task ProcessShouldDiscardEmptyMessage()
    {
        var function = new EventBridgeEmptyInvoiceProcessor(serviceMock);

        var lambdaContext = new TestLambdaContext();

        var message = CreateValidCloudWatchEvent(new EmptyInvoice());

        await function.Process(message, lambdaContext);

        serviceMock.Received().InitializeIoc();
        serviceMock.DidNotReceive().ProcessMessage(Arg.Any<string>());
    }

    private static CloudWatchEvent<T> CreateValidCloudWatchEvent<T>(T detail, string? eventId = null)
    {
        return new CloudWatchEvent<T>
        {
            Id = eventId ?? Guid.NewGuid().ToString(),
            Source = "test.source",
            DetailType = "TestDetail",
            Region = "us-east-1",
            Time = DateTime.UtcNow,
            Detail = detail
        };
    }
}
