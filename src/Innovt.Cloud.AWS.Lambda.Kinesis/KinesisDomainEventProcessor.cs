// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Lambda.Kinesis
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.KinesisEvents;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Core.Events;

namespace Innovt.Cloud.AWS.Lambda.Kinesis
{
    public abstract class KinesisDomainEventProcessor<TBody> : KinesisDataProcessorBase<TBody> where TBody : DomainEvent
    {
        protected KinesisDomainEventProcessor(ILogger logger) : base(logger)
        {
        }

        protected KinesisDomainEventProcessor()
        {
        }

        protected new virtual TBody DeserializeBody(string content, string partition)
        {
            return JsonSerializer.Deserialize<TBody>(content);
        }

        protected override async Task Handle(KinesisEvent message, ILambdaContext context)
        {
            Logger.Info($"Processing Kinesis Event With {message?.Records?.Count} records.");

            if (message?.Records == null) return;
            if (message.Records.Count == 0) return;

            using var activity = EventProcessorActivitySource.StartActivity(nameof(Handle));
            foreach (var record in message.Records)
            {
                Logger.Info($"Processing Kinesis Event message ID {record.EventId}.");
                    
                try
                {
                    Logger.Info("Reading Stream Content.");
                        
                    var body = await ParseRecord(record).ConfigureAwait(false);

                    if (body?.TraceId != null && activity != null)
                    {
                        activity.SetParentId(body.TraceId);
                    }

                    await ProcessMessage(body?.Body).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Error Processing Message from Kinesis Event. Developer, you should take care of it!. Message: Id={EventId}, PartitionKey= {PartitionKey}",
                        record.EventId, record.Kinesis.PartitionKey);
                    throw;
                }
            }
        }

        protected abstract Task ProcessMessage(TBody message);
    }
}