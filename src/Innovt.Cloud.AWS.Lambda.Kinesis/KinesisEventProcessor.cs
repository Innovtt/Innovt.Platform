using System;
using Amazon.Lambda.Core;
using Amazon.Lambda.KinesisEvents;
using Innovt.Core.CrossCutting.Log;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Innovt.Domain.Core.Streams;

namespace Innovt.Cloud.AWS.Lambda.Kinesis
{
    public abstract class KinesisEventProcessor<TBody> : EventProcessor<KinesisEvent> where TBody :class
    {
        protected KinesisEventProcessor(ILogger logger) : base(logger)
        {
        }
        protected KinesisEventProcessor() : base()
        {
        }

        protected virtual TBody DeserializeBody(string content, string partition)
        {
            return System.Text.Json.JsonSerializer.Deserialize<TBody>(content);
        }

        protected override async Task Handle(KinesisEvent kinesisEvent, ILambdaContext context)
        {
            Logger.Info($"Processing Kinesis Event With {kinesisEvent.Records?.Count} records.");

            if (kinesisEvent?.Records == null) return;
            if (kinesisEvent.Records.Count == 0) return;
         
            foreach (var record in kinesisEvent.Records)
            {
                Logger.Info($"Processing Kinesis Event message ID {record.EventId}.");
                
                try
                {    
                    Logger.Info("Reading Stream Content.");
                    
                    using var reader = new StreamReader(record.Kinesis.Data, Encoding.UTF8);

                    var content = await reader.ReadToEndAsync();

                    Logger.Info("Stream Content Read.");
                    
                    Logger.Info("Creating DataStream Message.");

                    var message = new DataStream<TBody>()
                    {
                        Body = DeserializeBody(content, record.Kinesis.PartitionKey),
                        EventId = record.EventId,
                        Partition = record.Kinesis.PartitionKey,
                        ApproximateArrivalTimestamp = record.Kinesis.ApproximateArrivalTimestamp
                    };
                    
                    Logger.Info("Invoking ProcessMessage.");
                    
                    await ProcessMessage(message);
                }
                catch(Exception ex)
                {
                    Logger.Error(ex,"Error Processing Message from Kinesis Event. Developer, you should take care of it!. Message: Id={EventId}, PartitionKey= {PartitionKey}",
                        record.EventId,record.Kinesis.PartitionKey);
                    throw;
                }
            }
        }

        protected abstract Task ProcessMessage(DataStream<TBody> message);
    }
}