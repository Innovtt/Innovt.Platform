using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Innovt.Cloud.Queue;
using Innovt.Core.CrossCutting.Log;

namespace Innovt.Cloud.AWS.Lambda
{
    public abstract class SqsEventProcessor<TBody> : EventProcessor<SQSEvent> where TBody : class
    {
        protected SqsEventProcessor(ILogger logger) : base(logger)
        {
        }

        protected SqsEventProcessor() : base()
        {
        }

        public override async Task Handle(SQSEvent sqsEvent, ILambdaContext context)
        {
            Logger.Info($"Processing Sqs event With {sqsEvent.Records?.Count} records.");

            if (sqsEvent?.Records == null) return;
            if (sqsEvent.Records.Count == 0) return;

            foreach (var record in sqsEvent.Records)
            {
                Logger.Info($"Processing SQS Event message ID {record.MessageId}.");

                var message = new QueueMessage<TBody>(record.Attributes)
                {
                    MessageId = record.MessageId,
                    ReceiptHandle = record.ReceiptHandle,
                    Body = JsonSerializer.Deserialize<TBody>(record.Body)
                };

                await ProcessMessage(message);

                Logger.Info($"SQS Event message ID {record.MessageId} Processed.");
            }
        }

        protected abstract Task ProcessMessage(QueueMessage<TBody> message);
    }
}
