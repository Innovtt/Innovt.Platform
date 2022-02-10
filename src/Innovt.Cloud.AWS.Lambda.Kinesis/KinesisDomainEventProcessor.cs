// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Lambda.Kinesis
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Innovt.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Innovt.Cloud.AWS.Lambda.Kinesis
{
    public abstract class KinesisDomainEventProcessor<TBody> : KinesisDomainEventProcessorBatch<TBody> where TBody : DomainEvent
    {
        protected KinesisDomainEventProcessor(ILogger logger) : base(logger)
        {
        }

        protected KinesisDomainEventProcessor()
        {

        }

        protected override async Task<IList<BatchFailureResponse>> ProcessMessages(IList<TBody> messages)
        {
            if (messages == null) throw new ArgumentNullException(nameof(messages));

            var response = new List<BatchFailureResponse>();

            foreach (var message in messages)
            {
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
                catch (Exception)
                {
                    response.Add(new BatchFailureResponse(message.EventId));
                }
            }

            return response;
        }

        protected abstract Task ProcessMessage(TBody message);
    }
}