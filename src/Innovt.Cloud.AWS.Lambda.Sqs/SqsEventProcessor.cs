// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Sqs

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Innovt.Cloud.Queue;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Serialization;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Lambda.Sqs;

/// <summary>
///     If you're using this feature with a FIFO queue, your function should stop processing messages after the first
///     failure and return all failed and unprocessed messages in batchItemFailures. This helps preserve the ordering of
///     messages in your queue.
/// </summary>
/// <typeparam name="TBody"></typeparam>
/// 

// <summary>
/// The abstract SqsEventProcessor class serves as the base class for processing Amazon SQS events in AWS Lambda functions.
/// </summary>
/// <typeparam name="TBody">The type of the message body within SQS event records.</typeparam>
public abstract class SqsEventProcessor<TBody> : EventProcessor<SQSEvent, BatchFailureResponse> where TBody : class
{
    private readonly bool isFifo;
    private ISerializer serializer;

    /// <summary>
    /// Initializes a new instance of the SqsEventProcessor class with the specified FIFO queue setting and report batch failures setting.
    /// </summary>
    /// <param name="isFifo">A boolean indicating whether the SQS queue is FIFO (First-In-First-Out).</param>
    /// <param name="reportBatchFailures">A boolean indicating whether to report batch processing failures.</param>
    protected SqsEventProcessor(bool isFifo = false, bool reportBatchFailures = false)
    {
        this.isFifo = isFifo;
        ReportBatchFailures = reportBatchFailures;
    }

    /// <summary>
    /// Initializes a new instance of the SqsEventProcessor class with the specified logger, FIFO queue setting, and report batch failures setting.
    /// </summary>
    /// <param name="logger">The logger used for logging events and messages.</param>
    /// <param name="isFifo">A boolean indicating whether the SQS queue is FIFO (First-In-First-Out).</param>
    /// <param name="reportBatchFailures">A boolean indicating whether to report batch processing failures.</param>
    protected SqsEventProcessor(ILogger logger, bool isFifo = false, bool reportBatchFailures = false) : base(logger)
    {
        this.isFifo = isFifo;
        ReportBatchFailures = reportBatchFailures;
    }


    /// <summary>
    /// Initializes a new instance of the SqsEventProcessor class with the specified logger, serializer, FIFO queue setting, and report batch failures setting.
    /// </summary>
    /// <param name="logger">The logger used for logging events and messages.</param>
    /// <param name="serializer">The serializer used for message deserialization.</param>
    /// <param name="isFifo">A boolean indicating whether the SQS queue is FIFO (First-In-First-Out).</param>
    /// <param name="reportBatchFailures">A boolean indicating whether to report batch processing failures.</param>
    protected SqsEventProcessor(ILogger logger, ISerializer serializer, bool isFifo = false,
        bool reportBatchFailures = false) : this(logger, isFifo, reportBatchFailures)
    {
        Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    /// <summary>
    /// Gets or sets a boolean indicating whether to report batch processing failures.
    /// </summary>
    protected bool ReportBatchFailures { get; set; }

    /// <summary>
    /// Gets or sets the serializer used for message deserialization.
    /// </summary>
    private ISerializer Serializer
    {
        get { return serializer ??= new JsonSerializer(); }

        set => serializer = value;
    }

    /// <summary>
    /// Handles the processing of an incoming SQSEvent, which may contain multiple SQS event records.
    /// </summary>
    /// <param name="message">The SQSEvent containing one or more SQS event records.</param>
    /// <param name="context">The ILambdaContext providing information about the Lambda function's execution environment.</param>
    /// <returns>A Task representing the asynchronous processing operation.</returns>
    protected override async Task<BatchFailureResponse> Handle(SQSEvent message, ILambdaContext context)
    {
        Logger.Info($"Processing Sqs event With {message?.Records?.Count} records.");

        using var watcher = new StopWatchHelper(Logger, nameof(Handle));

        var response = new BatchFailureResponse();

        if (message?.Records == null || message.Records.Count == 0) return response;

        using var activity = EventProcessorActivitySource.StartActivity(nameof(Handle));
        activity?.SetTag("SqsMessageRecordsCount", message?.Records?.Count);

        if (message.Records is null)
            return response;
        
        var processedMessages = new List<string>();

        foreach (var record in message.Records)
        {
            try
            {
                Logger.Info($"Processing SQS Event message ID {record.MessageId}.");

                var queueMessage = new QueueMessage<TBody>
                {
                    MessageId = record.MessageId,
                    ReceiptHandle = record.ReceiptHandle,
                    Attributes = record.Attributes,
                    Body = Serializer.DeserializeObject<TBody>(record.Body)
                };

                if (record.Attributes is not null)
                {
                    queueMessage.ParseQueueAttributes(record.Attributes);

                    record.Attributes.TryGetValue("TraceId", out var traceId);

                    if (traceId is not null) activity?.SetParentId(traceId);
                }

                activity?.SetTag("SqsMessageId", queueMessage.MessageId);
                activity?.SetTag("SqsMessageApproximateFirstReceiveTimestamp",
                    queueMessage.ApproximateFirstReceiveTimestamp);
                activity?.SetTag("SqsMessageApproximateReceiveCount", queueMessage.ApproximateReceiveCount);
                activity?.AddBaggage("Message.ElapsedTimeBeforeAttendedInMilliseconds",
                    $"{queueMessage.ApproximateFirstReceiveTimestamp.GetValueOrDefault()}");

                await ProcessMessage(queueMessage).ConfigureAwait(false);

                processedMessages.Add(record.MessageId);

                Logger.Info($"SQS Event message ID {record.MessageId} Processed.");
            }
            catch (Exception ex)
            {
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);

                if (!ReportBatchFailures)
                    throw;

                Logger.Warning($"SQS Event message ID {record.MessageId} will be returned as item failure.");
                Logger.Error(ex, $"Exception for message ID {record.MessageId}.");


                if (isFifo)
                {
                    response.AddItems(GetRemainingMessages(message, processedMessages));
                    break;
                }

                response.AddItem(record.MessageId);
            }
        }

        return response;
    }

    /// <summary>
    /// Gets the remaining messages in the SQSEvent that were not processed successfully.
    /// </summary>
    /// <param name="message">The SQSEvent containing one or more SQS event records.</param>
    /// <param name="processedMessages">A list of processed message IDs.</param>
    /// <returns>An IEnumerable of message IDs representing the remaining unprocessed messages.</returns>
    private static IEnumerable<string> GetRemainingMessages(SQSEvent message, IList<string> processedMessages)
    {
        return message.Records.Where(r => !processedMessages.Contains(r.MessageId)).Distinct().Select(r => r.MessageId);
    }

    /// <summary>
    /// Handles the processing of an individual SQS message.
    /// </summary>
    /// <param name="message">The QueueMessage object containing information about the SQS message.</param>
    /// <returns>A Task representing the asynchronous processing operation.</returns>
    protected abstract Task ProcessMessage(QueueMessage<TBody> message);
}