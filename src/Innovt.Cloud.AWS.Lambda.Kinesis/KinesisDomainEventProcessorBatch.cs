// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Kinesis

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.KinesisEvents;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Core.Events;

namespace Innovt.Cloud.AWS.Lambda.Kinesis;

/// <summary>
///     Represents a base class for processing batches of domain-specific Kinesis events, where each event is of type
/// </summary>
/// <typeparam name="TBody">The type of domain-specific events to process.</typeparam>
public abstract class KinesisDomainEventProcessorBatch<TBody> : KinesisProcessorBase<TBody> where TBody : DomainEvent
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="KinesisDomainEventProcessorBatch{TBody}" /> class with optional
    ///     logging and batch failure reporting.
    /// </summary>
    /// <param name="logger">An optional logger for recording processing information.</param>
    /// <param name="reportBatchFailures">Specifies whether to report batch processing failures.</param>
    protected KinesisDomainEventProcessorBatch(ILogger logger, bool reportBatchFailures = false) : base(logger,
        reportBatchFailures)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KinesisDomainEventProcessorBatch{TBody}" /> class with optional batch
    ///     failure reporting.
    /// </summary>
    /// <param name="reportBatchFailures">Specifies whether to report batch processing failures.</param>
    protected KinesisDomainEventProcessorBatch(bool reportBatchFailures = false) : base(reportBatchFailures)
    {
    }

    /// <summary>
    ///     Deserializes the content of a Kinesis message into an instance of type <typeparamref name="TBody" />.
    /// </summary>
    /// <param name="content">The content of the Kinesis message.</param>
    /// <param name="partition">The partition associated with the message.</param>
    /// <returns>An instance of type <typeparamref name="TBody" /> representing the deserialized message.</returns>
    protected override TBody DeserializeBody(string content, string partition)
    {
        return JsonSerializer.Deserialize<TBody>(content);
    }

    /// <summary>
    ///     Creates a batch of domain-specific events from a collection of Kinesis event records.
    /// </summary>
    /// <param name="messageRecords">The collection of Kinesis event records to process.</param>
    /// <returns>A list of domain-specific events of type <typeparamref name="TBody" /> created from the event records.</returns>
    private async Task<IList<TBody>> CreateBatchMessages(IEnumerable<KinesisEvent.KinesisEventRecord> messageRecords)
    {
        ArgumentNullException.ThrowIfNull(messageRecords);

        var dataStreams = new List<TBody>();

        foreach (var record in messageRecords) dataStreams.Add(await ParseRecord(record).ConfigureAwait(false));

        return dataStreams;
    }

    /// <summary>
    ///     Handles a Kinesis event by processing a batch of domain-specific events.
    /// </summary>
    /// <param name="message">The Kinesis event containing multiple records.</param>
    /// <param name="context">The Lambda execution context.</param>
    /// <returns>A <see cref="BatchFailureResponse" /> containing information about failed event processing.</returns>
    protected override async Task<BatchFailureResponse> Handle(KinesisEvent message, ILambdaContext context)
    {
        Logger.Info($"Processing Kinesis Event With {message?.Records?.Count} records.");

        using var activity = EventProcessorActivitySource.StartActivity();
        activity?.SetTag("Message.Records", message?.Records?.Count);

        if (message?.Records == null || message.Records.Count == 0) return new BatchFailureResponse();

        Logger.Info($"Processing Kinesis Event With {message?.Records?.Count} records.");
        var batchMessages = await CreateBatchMessages(message.Records).ConfigureAwait(false);
        Logger.Info($"Processing Kinesis Event With {message?.Records?.Count} records.");
        activity?.SetTag("BatchMessagesCount", batchMessages.Count);

        return await ProcessMessages(batchMessages).ConfigureAwait(false);
    }

    /// <summary>
    ///     Processes a batch of domain-specific events of type <typeparamref name="TBody" />.
    /// </summary>
    /// <param name="messages">The batch of domain-specific events to process.</param>
    /// <returns>A <see cref="BatchFailureResponse" /> containing information about failed event processing.</returns>
    protected abstract Task<BatchFailureResponse> ProcessMessages(IList<TBody> messages);
}