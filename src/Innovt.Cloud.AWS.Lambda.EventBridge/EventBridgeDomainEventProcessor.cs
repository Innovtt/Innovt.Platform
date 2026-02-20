// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.EventBridge

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Innovt.Cloud.AWS.Lambda.EventBridge.Events;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Core.Events;
using Innovt.Domain.Core.Streams;

namespace Innovt.Cloud.AWS.Lambda.EventBridge;

/// <summary>
///     Represents a base class for processing EventBridge events with domain event content of type
///     <typeparamref name="TBody" />.
/// </summary>
/// <typeparam name="TBody">The type of domain event contained in the EventBridge event detail.</typeparam>
public abstract class EventBridgeDomainEventProcessor<TBody> : EventProcessor<EventBridgeMessage> where TBody : DomainEvent
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="EventBridgeDomainEventProcessor{TBody}" /> class with optional
    ///     logging.
    /// </summary>
    /// <param name="logger">An optional logger for recording processing information.</param>
    protected EventBridgeDomainEventProcessor(ILogger logger) : base(logger)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="EventBridgeDomainEventProcessor{TBody}" /> class.
    /// </summary>
    protected EventBridgeDomainEventProcessor()
    {
    }

    /// <summary>
    ///     Handles the processing of an incoming EventBridge domain event.
    /// </summary>
    /// <param name="message">The CloudWatchEvent containing the domain event detail.</param>
    /// <param name="context">The ILambdaContext providing information about the Lambda function's execution environment.</param>
    /// <returns>A Task representing the asynchronous processing operation.</returns>
    protected override async Task Handle([NotNull]EventBridgeMessage message, ILambdaContext context)
    {
        Logger.Info($"Processing EventBridge event from Source={message.Source}, DetailType={message.DetailType}.");

        using var activity = EventProcessorActivitySource.StartActivity();
        activity?.SetTag("EventBridge.Id", message.Id);
        activity?.SetTag("EventBridge.Source", message.Source);
        activity?.SetTag("EventBridge.DetailType", message.DetailType);
        activity?.SetTag("EventBridge.Region", message.Region);
        activity?.SetTag("EventBridge.Time", message.Time);
        
        var detail = JsonSerializer.Serialize(message.Detail);
        
        var body = DeserializeBody(detail, message.DetailType);

        if (body is null)
        {
            Logger.Warning("EventBridge event detail is null. Skipping processing.");
            return;
        }

        body.EventId ??= message.Id;
        body.Partition ??= message.Source;
        body.ApproximateArrivalTimestamp = message.Time;
        body.TraceId ??= activity?.Id;

        if (body.TraceId != null && activity?.ParentId is null)
            activity?.SetParentId(body.TraceId);

        if (body is IEmptyDataStream)
        {
            Logger.Warning($"Discarding empty message from Source={message.Source}. EventId={message.Id}");
            return;
        }

        Logger.Info($"Processing EventBridge EventId={message.Id}.");

        await ProcessMessage(body).ConfigureAwait(false);

        Logger.Info($"EventId={message.Id} from EventBridge processed.");
    }

    /// <summary>
    ///     Deserializes the content of a Kinesis event record into an instance of type <typeparamref name="TBody" />.
    /// </summary>
    /// <param name="content">The content of the Kinesis event record.</param>
    /// <param name="partition">The partition associated with the message.</param>
    /// <returns>An instance of type <typeparamref name="TBody" /> representing the deserialized content.</returns>
    protected virtual TBody? DeserializeBody(string content, string? partition = null)
    {
        return JsonSerializer.Deserialize<TBody>(content);
    }

    /// <summary>
    ///     Processes an individual EventBridge domain event of type <typeparamref name="TBody" />.
    /// </summary>
    /// <param name="message">The EventBridge domain event to process.</param>
    /// <returns>A task representing the asynchronous processing operation.</returns>
    protected abstract Task ProcessMessage([NotNull]TBody message);
}
