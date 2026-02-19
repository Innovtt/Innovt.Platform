// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.EventBridge

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Amazon.Lambda.CloudWatchEvents;
using Amazon.Lambda.Core;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Core.Streams;

namespace Innovt.Cloud.AWS.Lambda.EventBridge;

/// <summary>
///     Represents a base class for processing EventBridge events with content of type <typeparamref name="TBody" />.
/// </summary>
/// <typeparam name="TBody">The type of content contained in the EventBridge event detail.</typeparam>
public abstract class EventBridgeEventProcessor<TBody> : EventProcessor<CloudWatchEvent<TBody>>
    where TBody : class, IDataStream
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="EventBridgeEventProcessor{TBody}" /> class with optional logging.
    /// </summary>
    /// <param name="logger">An optional logger for recording processing information.</param>
    protected EventBridgeEventProcessor(ILogger logger) : base(logger)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="EventBridgeEventProcessor{TBody}" /> class.
    /// </summary>
    protected EventBridgeEventProcessor()
    {
    }

    /// <summary>
    ///     Handles the processing of an incoming EventBridge event.
    /// </summary>
    /// <param name="message">The CloudWatchEvent containing the event detail.</param>
    /// <param name="context">The ILambdaContext providing information about the Lambda function's execution environment.</param>
    /// <returns>A Task representing the asynchronous processing operation.</returns>
    protected override async Task Handle([NotNull]CloudWatchEvent<TBody> message, ILambdaContext context)
    {
        Logger.Info($"Processing EventBridge event from Source={message.Source}, DetailType={message.DetailType}.");

        using var activity = EventProcessorActivitySource.StartActivity();
        activity?.SetTag("EventBridge.Id", message.Id);
        activity?.SetTag("EventBridge.Source", message.Source);
        activity?.SetTag("EventBridge.DetailType", message.DetailType);
        activity?.SetTag("EventBridge.Region", message.Region);
        activity?.SetTag("EventBridge.Time", message.Time);

        var body = DeserializeDetail(message);

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

        body.PublishedAt = null;

        await ProcessMessage(body).ConfigureAwait(false);

        body.PublishedAt = DateTimeOffset.UtcNow;

        Logger.Info($"EventId={message.Id} from EventBridge processed.");
    }

    /// <summary>
    ///     Deserializes the event detail from the CloudWatchEvent. Override this method for custom deserialization.
    /// </summary>
    /// <param name="message">The CloudWatchEvent containing the event detail.</param>
    /// <returns>An instance of type <typeparamref name="TBody" /> representing the deserialized detail.</returns>
    protected virtual TBody DeserializeDetail([NotNull]CloudWatchEvent<TBody> message)
    {
        return message.Detail;
    }

    /// <summary>
    ///     Processes an individual EventBridge message of type <typeparamref name="TBody" />.
    /// </summary>
    /// <param name="message">The EventBridge message to process.</param>
    /// <returns>A task representing the asynchronous processing operation.</returns>
    protected abstract Task ProcessMessage(TBody message);
}
