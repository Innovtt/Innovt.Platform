// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Lambda.Kinesis
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.KinesisEvents;
using Innovt.Core.CrossCutting.Log;
using Innovt.Domain.Core.Streams;

namespace Innovt.Cloud.AWS.Lambda.Kinesis
{
    public abstract class KinesisDataProcessorBase<TBody> : EventProcessor<KinesisEvent> where TBody : class
    {

        protected KinesisDataProcessorBase(ILogger logger) : base(logger)
        {
        }

        protected KinesisDataProcessorBase()
        {
        }

        protected virtual DataStream<TBody> DeserializeBody(string content, string partition)
        {
            return JsonSerializer.Deserialize<DataStream<TBody>>(content);
        }


        protected async Task<DataStream<TBody>> ParseRecord(KinesisEvent.KinesisEventRecord record)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));

            Logger.Info($"Processing Kinesis Event message ID {record.EventId}.");

            using var activity = EventProcessorActivitySource.StartActivity(nameof(ParseRecord));
            activity?.SetTag("EventId", record.EventId);
            activity?.SetTag("EventName", record.EventName);
            activity?.SetTag("EventVersion", record.EventVersion);
            activity?.SetTag("EventSource", record.EventSource);

            try
            {
                Logger.Info("Reading Stream Content.");

                using var reader = new StreamReader(record.Kinesis.Data, Encoding.UTF8);

                var content = await reader.ReadToEndAsync().ConfigureAwait(false);

                Logger.Info("Stream Content Read.");

                Logger.Info("Creating DataStream Message.");

                var body = DeserializeBody(content, record.Kinesis.PartitionKey);

                if (body != null)
                {
                    body.EventId = record.EventId;
                    body.ApproximateArrivalTimestamp = record.Kinesis.ApproximateArrivalTimestamp;
                }

                Logger.Info("Invoking ProcessMessage.");

                return body;
            }
            catch (Exception ex)
            {
                Logger.Error(ex,
                    "Error Processing Message from Kinesis Event. Developer, you should take care of it!. Message: Id={EventId}, PartitionKey= {PartitionKey}",
                    record.EventId, record.Kinesis.PartitionKey);
                throw;
            }
        }
    }
}