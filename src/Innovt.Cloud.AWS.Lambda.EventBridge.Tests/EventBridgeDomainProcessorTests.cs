// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Kinesis.Tests

using Amazon.Lambda.TestUtilities;
using Innovt.Cloud.AWS.Lambda.EventBridge.Events;
using Innovt.Cloud.AWS.Lambda.EventBridge.Tests.Processors;
using Innovt.Domain.Core.Events;
using NSubstitute;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Lambda.EventBridge.Tests;

[TestFixture]
public class EventBridgeDomainProcessorTests
{
    [SetUp]
    public void Setup()
    {
        serviceMock = Substitute.For<IDomainEventServiceMock<DomainEvent>>();
    }

    private IDomainEventServiceMock<DomainEvent> serviceMock = null!;

    [Test]
    public async Task SimpleFunctionTest()
    {
        var domainServiceMock = Substitute.For<IDomainEventServiceMock<DomainEvent>>();

        var function = new EventBridgeDomainEventEmptyInvoiceProcessor(domainServiceMock);

        await function.Process(new EventBridgeMessage(), new TestLambdaContext());
        //should not receive the call because there is no records
        domainServiceMock.DidNotReceive().ProcessMessage(Arg.Any<DomainEvent>());
        
        Assert.Pass();
    }

    //
    [Test]
    public void ProcessThrowExceptionIfMessageIsNull()
    {
        var function = new EventBridgeDomainEventEmptyInvoiceProcessor(serviceMock);
    
        Assert.ThrowsAsync<ArgumentNullException>(async () => await function.Process(null!, null));
    }
    
    [Test]
    public void ProcessThrowExceptionIfContextIsNull()
    {
        var function = new EventBridgeDomainEventEmptyInvoiceProcessor(serviceMock);
    
        Assert.ThrowsAsync<ArgumentNullException>(async () => await function.Process(new EventBridgeMessage(), null));
    }
    
    [Test]
    public async Task ProcessShouldCallIoc()
    {
        var function = new EventBridgeDomainEventEmptyInvoiceProcessor(serviceMock);
    
        var lambdaContext = new TestLambdaContext();
    
        var message = new EventBridgeMessage();
    
        await function.Process(message, lambdaContext);
    
        serviceMock.Received().InicializeIoc();
        serviceMock.DidNotReceive().ProcessMessage(Arg.Any<DomainEvent>());
    }
    
    
    [Test]
    public async Task ProcessWithoutMessageReturnWithoutCallHandle()
    {
        var function = new EventBridgeDomainEventEmptyInvoiceProcessor(serviceMock);

        var lambdaContext = new TestLambdaContext();

        var message = new EventBridgeMessage();

        await function.Process(message, lambdaContext);

        serviceMock.DidNotReceive().ProcessMessage(Arg.Any<DomainEvent>());
    }

    [Test]
    public void ConstructorThrowsIfServiceIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new EventBridgeDomainEventEmptyInvoiceProcessor(null!));
    }

    [Test]
    public async Task ProcessDiscardsEmptyEventEvenWithPopulatedMessage()
    {
        var function = new EventBridgeDomainEventEmptyInvoiceProcessor(serviceMock);

        var message = new EventBridgeMessage
        {
            Detail = new { NetValue = 200.00m },
            Source = "com.example.invoice",
            Id = "abc-123",
            Time = DateTime.UtcNow,
            Region = "us-east-1",
            DetailType = "InvoiceCreated"
        };

        await function.Process(message, new TestLambdaContext());

        serviceMock.DidNotReceive().ProcessMessage(Arg.Any<DomainEvent>());
    }

    [Test]
    public async Task ProcessDiscardsEmptyEventWithJsonDetail()
    {
        var function = new EventBridgeDomainEventEmptyInvoiceProcessor(serviceMock);

        var message = new EventBridgeMessage
        {
            Detail = new { Key = "value", Nested = new { Inner = 42 } },
            Source = "com.example.orders",
            Id = "def-456",
            Time = DateTime.UtcNow,
            DetailType = "OrderPlaced"
        };

        await function.Process(message, new TestLambdaContext());

        serviceMock.DidNotReceive().ProcessMessage(Arg.Any<DomainEvent>());
    }

    [Test]
    public void InvoiceProcessorConstructorThrowsIfServiceIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new EventBridgeDomainEventInvoiceProcessor(null!));
    }

    [Test]
    public async Task InvoiceProcessorCallsProcessMessageWithDeserializedBody()
    {
        var invoiceServiceMock = Substitute.For<IDomainEventServiceMock<InvoiceDomainEvent>>();
        var function = new EventBridgeDomainEventInvoiceProcessor(invoiceServiceMock);

        var message = new EventBridgeMessage
        {
            Detail = new { NetValue = 100.50m },
            Source = "com.example.invoice",
            Id = "evt-001",
            Time = DateTime.UtcNow,
            DetailType = "InvoiceCreated"
        };

        await function.Process(message, new TestLambdaContext());

        invoiceServiceMock.Received(1).ProcessMessage(Arg.Is<InvoiceDomainEvent>(e => e.NetValue == 100.50m));
    }

    [Test]
    public async Task InvoiceProcessorHydratesEventIdFromMessage()
    {
        var invoiceServiceMock = Substitute.For<IDomainEventServiceMock<InvoiceDomainEvent>>();
        var function = new EventBridgeDomainEventInvoiceProcessor(invoiceServiceMock);

        var message = new EventBridgeMessage
        {
            Detail = new { NetValue = 50.00m },
            Source = "com.example.invoice",
            Id = "hydrated-event-id",
            Time = DateTime.UtcNow,
            DetailType = "InvoiceCreated"
        };

        await function.Process(message, new TestLambdaContext());

        invoiceServiceMock.Received(1).ProcessMessage(
            Arg.Is<InvoiceDomainEvent>(e => e.EventId == "hydrated-event-id"));
    }

    [Test]
    public async Task InvoiceProcessorHydratesPartitionFromSource()
    {
        var invoiceServiceMock = Substitute.For<IDomainEventServiceMock<InvoiceDomainEvent>>();
        var function = new EventBridgeDomainEventInvoiceProcessor(invoiceServiceMock);

        var message = new EventBridgeMessage
        {
            Detail = new { NetValue = 75.00m, Partition = (string?)null },
            Source = "com.example.billing",
            Id = "evt-002",
            Time = DateTime.UtcNow,
            DetailType = "InvoiceCreated"
        };

        await function.Process(message, new TestLambdaContext());

        invoiceServiceMock.Received(1).ProcessMessage(
            Arg.Is<InvoiceDomainEvent>(e => e.Partition == "com.example.billing"));
    }

    [Test]
    public async Task InvoiceProcessorSetsApproximateArrivalTimestamp()
    {
        var invoiceServiceMock = Substitute.For<IDomainEventServiceMock<InvoiceDomainEvent>>();
        var function = new EventBridgeDomainEventInvoiceProcessor(invoiceServiceMock);

        var expectedTime = new DateTime(2025, 6, 15, 10, 30, 0, DateTimeKind.Utc);

        var message = new EventBridgeMessage
        {
            Detail = new { NetValue = 25.00m },
            Source = "com.example.invoice",
            Id = "evt-003",
            Time = expectedTime,
            DetailType = "InvoiceCreated"
        };

        await function.Process(message, new TestLambdaContext());

        invoiceServiceMock.Received(1).ProcessMessage(
            Arg.Is<InvoiceDomainEvent>(e => e.ApproximateArrivalTimestamp == expectedTime));
    }

    [Test]
    public async Task InvoiceProcessorSkipsProcessingWhenDetailIsNull()
    {
        var invoiceServiceMock = Substitute.For<IDomainEventServiceMock<InvoiceDomainEvent>>();
        var function = new EventBridgeDomainEventInvoiceProcessor(invoiceServiceMock);

        var message = new EventBridgeMessage
        {
            Detail = null,
            Source = "com.example.invoice",
            Id = "evt-004",
            Time = DateTime.UtcNow,
            DetailType = "InvoiceCreated"
        };

        await function.Process(message, new TestLambdaContext());

        invoiceServiceMock.DidNotReceive().ProcessMessage(Arg.Any<InvoiceDomainEvent>());
    }
}