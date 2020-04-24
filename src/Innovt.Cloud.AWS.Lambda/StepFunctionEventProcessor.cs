using Amazon.Lambda.Core;
using Amazon.Lambda.KinesisEvents;
using Innovt.Core.CrossCutting.Log;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Innovt.Cloud.AWS.Lambda
{
    public abstract class StepFunctionEventProcessor<TBody> : EventProcessor<KinesisEvent> where TBody : class
    {
        //protected KinesisEventProcessor(ILogger logger) : base(logger)
        //{
        //}

        //protected KinesisEventProcessor() : base()
        //{

        //}

        //public override async Task Handle(KinesisEvent kinesisEvent, ILambdaContext context)
        //{ 
        //    Logger.Info($"Processing Kinesis Event With {kinesisEvent.Records?.Count} records.");

        //    if (kinesisEvent?.Records == null) return;
        //    if (kinesisEvent.Records.Count == 0) return;

        //    foreach (var record in kinesisEvent.Records)
        //    {
        //        Logger.Info($"Processing Kinesis Event message ID {record.EventId}.");

        //        Logger.Info("Reading Stream Content.");

        //        try
        //        {
        //            using var reader = new StreamReader(record.Kinesis.Data, Encoding.UTF8);

        //            var content = reader.ReadToEnd();

        //            Logger.Info("Stream Content Readed.");

        //            var message = System.Text.Json.JsonSerializer.Deserialize<TBody>(content);

        //            await ProcessMessage(message);
        //        }
        //        catch
        //        {
        //            Logger.Warning("Error Processing Message from Kinesis Event. Developer, you should take care of it!");
        //        }
        //    }
        //}

        //protected abstract Task ProcessMessage(TBody message);
    }
}