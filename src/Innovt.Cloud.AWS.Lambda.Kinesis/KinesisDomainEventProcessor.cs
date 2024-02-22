// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Lambda.Kinesis

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Innovt.Domain.Core.Events;

namespace Innovt.Cloud.AWS.Lambda.Kinesis;

/// <summary>
///     Represents a base class for processing domain-specific Kinesis events, where each event is of type
///     <typeparamref name="TBody" />.
/// </summary>
/// <typeparam name="TBody">The type of domain-specific events to process.</typeparam>
public abstract class KinesisDomainEventProcessor<TBody> : KinesisDomainEventProcessorBatch<TBody>
    where TBody : DomainEvent
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="KinesisDomainEventProcessor{TBody}" /> class with optional logging and
    ///     batch failure reporting.
    /// </summary>
    /// <param name="logger">An optional logger for recording processing information.</param>
    /// <param name="reportBatchFailures">Specifies whether to report batch processing failures.</param>
    protected KinesisDomainEventProcessor(ILogger logger, bool reportBatchFailures = false) : base(logger,
        reportBatchFailures)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KinesisDomainEventProcessor{TBody}" /> class with optional batch
    ///     failure reporting.
    /// </summary>
    /// <param name="reportBatchFailures">Specifies whether to report batch processing failures.</param>
    protected KinesisDomainEventProcessor(bool reportBatchFailures = false) : base(reportBatchFailures)
    {
    }


    /// <summary>
    ///     Processes a batch of domain-specific events of type <typeparamref name="TBody" />.
    /// </summary>
    /// <param name="messages">The batch of domain-specific events to process.</param>
    /// <returns>A <see cref="BatchFailureResponse" /> containing information about failed event processing.</returns>
    protected override async Task<BatchFailureResponse> ProcessMessages(IList<TBody> messages)
    {
        if (messages == null) throw new ArgumentNullException(nameof(messages));

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

                await ProcessMessage(message).ConfigureAwait(false);

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
    ///     Processes a single domain-specific event of type <typeparamref name="TBody" />.
    /// </summary>
    /// <param name="message">The domain-specific event to process.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task ProcessMessage(TBody message);
}