// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Kinesis

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.KinesisEvents;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Innovt.Domain.Core.Streams;

namespace Innovt.Cloud.AWS.Lambda.Kinesis;

/// <summary>
///     Represents a base class for processing Kinesis events with content of type <typeparamref name="TBody" />.
/// </summary>
/// <typeparam name="TBody">The type of content contained in the Kinesis events.</typeparam>
public abstract class KinesisProcessorBase<TBody> : EventProcessor<KinesisEvent, BatchFailureResponse>
    where TBody : IDataStream
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="KinesisProcessorBase{TBody}" /> class with optional logging and batch
    ///     failure reporting.
    /// </summary>
    /// <param name="logger">An optional logger for recording processing information.</param>
    /// <param name="reportBatchFailures">Specifies whether to report batch processing failures.</param>
    protected KinesisProcessorBase(ILogger logger, bool reportBatchFailures = false) : base(logger)
    {
        ReportBatchFailures = reportBatchFailures;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KinesisProcessorBase{TBody}" /> class with optional batch failure
    ///     reporting.
    /// </summary>
    /// <param name="reportBatchFailures">Specifies whether to report batch processing failures.</param>
    protected KinesisProcessorBase(bool reportBatchFailures = false)
    {
        ReportBatchFailures = reportBatchFailures;
    }

    /// <summary>
    ///     Gets or sets a value indicating whether batch processing failures should be reported.
    /// </summary>
    protected bool ReportBatchFailures { get; set; }

    /// <summary>
    ///     Checks if a Kinesis message of type <typeparamref name="TBody" /> is empty and should be discarded.
    /// </summary>
    /// <param name="message">The Kinesis message to check.</param>
    /// <returns>True if the message is empty and should be discarded; otherwise, false.</returns>
    protected bool IsEmptyMessage(TBody message)
    {
        return message is IEmptyDataStream;
    }

    /// <summary>
    ///     Parses a Kinesis event record and returns its content as an instance of type <typeparamref name="TBody" />.
    /// </summary>
    /// <param name="record">The Kinesis event record to parse.</param>
    /// <returns>An instance of type <typeparamref name="TBody" /> representing the parsed content.</returns>
    protected async Task<TBody> ParseRecord(KinesisEvent.KinesisEventRecord record)
    {
        if (record == null) throw new ArgumentNullException(nameof(record));
        
        if (record.Kinesis.Data is null)
            throw new CriticalException($"Kinesis Data for EventId {record.EventId} is null");

        Logger.Info($"Processing Kinesis Event message ID {record.EventId}.");

        using var activity = EventProcessorActivitySource.StartActivity();
        activity?.SetTag("Kinesis.EventId", record.EventId);
        activity?.SetTag("Kinesis.EventName", record.EventName);
        activity?.SetTag("Kinesis.EventVersion", record.EventVersion);
        activity?.SetTag("Kinesis.EventSource", record.EventSource);
        activity?.SetTag("Kinesis.PartitionKey", record.Kinesis.PartitionKey);
        activity?.SetTag("Kinesis.ApproximateArrivalTimestamp", record.Kinesis.ApproximateArrivalTimestamp);
        activity?.AddBaggage("Message.ElapsedTimeBeforeAttendedInMilliseconds",
            $"{DateTime.UtcNow.Subtract(record.Kinesis.ApproximateArrivalTimestamp).TotalMilliseconds}");
        activity?.AddBaggage("Message.ElapsedTimeBeforeAttendedInMinutes",
            $"{DateTime.UtcNow.Subtract(record.Kinesis.ApproximateArrivalTimestamp).TotalMinutes}");

        Logger.Info("Reading Stream Content.");

        using var reader = new StreamReader(record.Kinesis.Data, Encoding.UTF8);

        var content = await reader.ReadToEndAsync().ConfigureAwait(false);

        Logger.Info("Deserializing Body Message.");

        var body = DeserializeBody(content, record.Kinesis.PartitionKey);

        Logger.Info("Body message deserialized.");

        if (body != null)
        {
            body.EventId = record.EventId;
            body.ApproximateArrivalTimestamp = record.Kinesis.ApproximateArrivalTimestamp;
            body.Partition ??= record.Kinesis.PartitionKey;
            body.TraceId ??= activity?.Id;
        }

        if (body?.TraceId != null && activity?.ParentId is null) activity?.SetParentId(body.TraceId);

        return body;
    }

    /// <summary>
    ///     Deserializes the content of a Kinesis event record into an instance of type <typeparamref name="TBody" />.
    /// </summary>
    /// <param name="content">The content of the Kinesis event record.</param>
    /// <param name="partition">The partition associated with the message.</param>
    /// <returns>An instance of type <typeparamref name="TBody" /> representing the deserialized content.</returns>
    protected abstract TBody DeserializeBody(string content, string partition);
}