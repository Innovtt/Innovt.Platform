// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Kinesis

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Innovt.Domain.Core.Streams;

namespace Innovt.Cloud.AWS.Lambda.Kinesis;

/// <summary>
///     Represents a base class for processing Kinesis data streams in batch, where each batch consists of messages of type
///     <typeparamref name="TBody" />.
/// </summary>
/// <typeparam name="TBody">The type of messages in the data stream.</typeparam>
public abstract class KinesisDataProcessor<TBody> : KinesisDataProcessorBatch<TBody> where TBody : class, IDataStream
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="KinesisDataProcessor{TBody}" /> class with optional logging and batch
    ///     failure reporting.
    /// </summary>
    /// <param name="logger">An optional logger for recording processing information.</param>
    /// <param name="reportBatchFailures">Specifies whether to report batch processing failures.</param>
    protected KinesisDataProcessor(ILogger logger, bool reportBatchFailures = false) : base(logger, reportBatchFailures)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KinesisDataProcessor{TBody}" /> class with optional batch failure
    ///     reporting.
    /// </summary>
    /// <param name="reportBatchFailures">Specifies whether to report batch processing failures.</param>
    protected KinesisDataProcessor(bool reportBatchFailures = false) : base(reportBatchFailures)
    {
    }

    /// <summary>
    ///     Processes a batch of Kinesis messages represented as a list of <typeparamref name="TBody" /> objects.
    /// </summary>
    /// <param name="messages">The list of Kinesis messages to process.</param>
    /// <returns>A <see cref="BatchFailureResponse" /> containing information about failed message processing.</returns>
    protected override async Task<BatchFailureResponse> ProcessMessages(IList<TBody> messages)
    {
        ArgumentNullException.ThrowIfNull(messages);

        var response = new BatchFailureResponse();

        foreach (var message in messages)
            try
            {
                if (message is null)
                    throw new CriticalException("Invalid message. The message from kinesis can't be null.");

                if (IsEmptyMessage(message))
                {
                    Logger.Warning($"Discarding message from partition {message.Partition}. EventId={message.EventId}");
                    continue;
                }

                Logger.Info($"Processing Kinesis EventId={message.EventId}.");

                message.PublishedAt = null;

                await ProcessMessage(message).ConfigureAwait(false);

                message.PublishedAt = DateTimeOffset.UtcNow;

                Logger.Info($"EventId={message.EventId} from Kinesis processed.");
            }
            catch (Exception ex)
            {
                if (!ReportBatchFailures)
                    throw;

                Logger.Error(ex, $"Exception for message ID {message?.EventId}.");

                response.AddItem(message?.EventId);
            }

        return response;
    }

    /// <summary>
    ///     Processes an individual Kinesis message of type <typeparamref name="TBody" />.
    /// </summary>
    /// <param name="message">The Kinesis message to process.</param>
    /// <returns>A task representing the asynchronous processing operation.</returns>
    protected abstract Task ProcessMessage(TBody message);
}