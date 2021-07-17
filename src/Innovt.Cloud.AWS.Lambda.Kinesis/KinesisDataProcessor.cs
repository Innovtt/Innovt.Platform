// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Lambda.Kinesis
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.KinesisEvents;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Core.Streams;

namespace Innovt.Cloud.AWS.Lambda.Kinesis
{
    public abstract class KinesisDataProcessor<TBody> : KinesisDataProcessorBase<TBody> where TBody : class
    {
        protected KinesisDataProcessor(ILogger logger) : base(logger)
        {
        }
        protected KinesisDataProcessor()
        {
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

                var body = await base.ParseRecord(record).ConfigureAwait(false);

                if (body?.TraceId !=null && activity !=null)
                {
                    activity.SetParentId(body.TraceId);
                }

                Logger.Info("Invoking ProcessMessage.");

                using var processActivity = EventProcessorActivitySource.StartActivity(nameof(ProcessMessage));
                processActivity?.SetTag("KinesisEventId", record?.EventId);
                processActivity?.SetTag("KinesisEventName", record?.EventName);
                processActivity?.SetTag("KinesisPartitionKey", record?.Kinesis?.PartitionKey);
                processActivity?.SetTag("KinesisApproximateArrivalTimestamp", record?.Kinesis?.ApproximateArrivalTimestamp);
                processActivity?.SetTag("BodyEventId", body?.EventId);
                processActivity?.SetTag("BodyPartition", body?.Partition);
                processActivity?.SetTag("BodyVersion", body?.Version);
                processActivity?.SetTag("BodyVersion", body?.TraceId);

                await ProcessMessage(body).ConfigureAwait(false);
            }
        }

        protected abstract Task ProcessMessage(DataStream<TBody> message);
    }
}