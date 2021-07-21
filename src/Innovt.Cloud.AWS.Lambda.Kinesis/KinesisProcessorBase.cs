// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Lambda.Kinesis
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Amazon.Lambda.KinesisEvents;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Core.Streams;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Innovt.Core.Exceptions;

namespace Innovt.Cloud.AWS.Lambda.Kinesis
{
    public abstract class KinesisProcessorBase<TBody> : EventProcessor<KinesisEvent> where TBody : IDataStream
    {
        protected KinesisProcessorBase(ILogger logger) : base(logger)
        {
        }

        protected KinesisProcessorBase()
        {
        }
        
        protected async Task<TBody> ParseRecord(KinesisEvent.KinesisEventRecord record)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));
            if (record.Kinesis.Data is null) throw new CriticalException($"Kinesis Data for EventId {record.EventId} is null");

            Logger.Info($"Processing Kinesis Event message ID {record.EventId}.");

            using var activity = EventProcessorActivitySource.StartActivity(nameof(ParseRecord));
            activity?.SetTag("Kinesis.EventId", record.EventId);
            activity?.SetTag("Kinesis.EventName", record.EventName);
            activity?.SetTag("Kinesis.EventVersion", record.EventVersion);
            activity?.SetTag("Kinesis.EventSource", record.EventSource);
            activity?.SetTag("Kinesis.PartitionKey", record.Kinesis?.PartitionKey);
            activity?.SetTag("Kinesis.ApproximateArrivalTimestamp", record.Kinesis?.ApproximateArrivalTimestamp);
            

            Logger.Info("Reading Stream Content.");

            using var reader = new StreamReader(record.Kinesis.Data, Encoding.UTF8);

            var content = await reader.ReadToEndAsync().ConfigureAwait(false);

            Logger.Info("Stream content read finished.");

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

            if (body?.TraceId != null && activity?.ParentId is null)
            {
                activity?.SetParentId(body.TraceId);
            }
            return body;
        }

        protected abstract TBody DeserializeBody(string content, string partition);
    }
}