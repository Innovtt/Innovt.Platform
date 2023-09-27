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
using Innovt.Domain.Core.Streams;

namespace Innovt.Cloud.AWS.Lambda.Kinesis;

/// <summary>
/// Represents a base class for processing Kinesis data streams in batch, where each batch consists of messages of type <typeparamref name="TBody"/>.
/// </summary>
/// <typeparam name="TBody">The type of messages in the data stream.</typeparam>
public abstract class KinesisDataProcessorBatch<TBody> : KinesisProcessorBase<TBody> where TBody : class, IDataStream
{

    /// <summary>
    /// Initializes a new instance of the <see cref="KinesisDataProcessorBatch{TBody}"/> class with optional logging and batch failure reporting.
    /// </summary>
    /// <param name="logger">An optional logger for recording processing information.</param>
    /// <param name="reportBatchFailures">Specifies whether to report batch processing failures.</param>
    protected KinesisDataProcessorBatch(ILogger logger, bool reportBatchFailures = false) : base(logger,
        reportBatchFailures)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="KinesisDataProcessorBatch{TBody}"/> class with optional batch failure reporting.
    /// </summary>
    /// <param name="reportBatchFailures">Specifies whether to report batch processing failures.</param>    
    protected KinesisDataProcessorBatch(bool reportBatchFailures = false) : base(reportBatchFailures)
    {
    }

    /// <summary>
    /// Deserializes the content of a Kinesis message into an instance of type <typeparamref name="TBody"/>.
    /// </summary>
    /// <param name="content">The content of the Kinesis message.</param>
    /// <param name="partition">The partition associated with the message.</param>
    /// <returns>An instance of type <typeparamref name="TBody"/> representing the deserialized message.</returns>
    protected override TBody DeserializeBody(string content, string partition)
    {
        return JsonSerializer.Deserialize<TBody>(content);
    }


    /// <summary>
    /// Creates a batch of messages from a collection of Kinesis event records.
    /// </summary>
    /// <param name="messageRecords">The collection of Kinesis event records to process.</param>
    /// <returns>A list of messages of type <typeparamref name="TBody"/> created from the event records.</returns>
    private async Task<IList<TBody>> CreateBatchMessages(IEnumerable<KinesisEvent.KinesisEventRecord> messageRecords)
    {
        if (messageRecords == null) throw new ArgumentNullException(nameof(messageRecords));

        var dataStreams = new List<TBody>();

        foreach (var record in messageRecords) dataStreams.Add(await ParseRecord(record).ConfigureAwait(false));

        return dataStreams;
    }

    /// <summary>
    /// Handles a Kinesis event by processing a batch of messages.
    /// </summary>
    /// <param name="message">The Kinesis event containing multiple records.</param>
    /// <param name="context">The Lambda execution context.</param>
    /// <returns>A <see cref="BatchFailureResponse"/> containing information about failed message processing.</returns>
    protected override async Task<BatchFailureResponse> Handle(KinesisEvent message, ILambdaContext context)
    {
        Logger.Info($"Processing Kinesis Event With {message?.Records?.Count} records.");

        using var activity = EventProcessorActivitySource.StartActivity(nameof(Handle));
        activity?.SetTag("Message.Records", message?.Records?.Count);

        if (message?.Records == null) return new BatchFailureResponse();
        if (message.Records.Count == 0) return new BatchFailureResponse();

        Logger.Info($"Processing Kinesis Event With {message?.Records?.Count} records.");
        var batchMessages = await CreateBatchMessages(message.Records).ConfigureAwait(false);
        Logger.Info($"Processing Kinesis Event With {message?.Records?.Count} records.");
        activity?.SetTag("BatchMessagesCount", batchMessages.Count);

        return await ProcessMessages(batchMessages).ConfigureAwait(false);
    }

    /// <summary>
    /// Processes a batch of messages of type <typeparamref name="TBody"/>.
    /// </summary>
    /// <param name="messages">The batch of messages to process.</param>
    /// <returns>A <see cref="BatchFailureResponse"/> containing information about failed message processing.</returns>
    protected abstract Task<BatchFailureResponse> ProcessMessages(IList<TBody> messages);
}