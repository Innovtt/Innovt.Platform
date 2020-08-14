using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Innovt.Cloud.Queue;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Serialization;

namespace Innovt.Cloud.AWS.Lambda
{
    public abstract class SqsEventProcessor<TBody> : EventProcessor<SQSEvent> where TBody : class
    {

        private ISerializer serializer;

        protected SqsEventProcessor(ILogger logger) : base(logger)
        {
        }

        protected SqsEventProcessor(ILogger logger, ISerializer serializer) : base(logger)
        {
            this.serializer = serializer ?? throw new System.ArgumentNullException(nameof(serializer));
        }

        protected SqsEventProcessor() : base()
        {
        }

        protected ISerializer Serializer
        {
            get
            {
                if (serializer == null)
                    serializer = new Innovt.Core.Serialization.JsonSerializer();

                return serializer;
            }

            set
            {
                this.serializer = value;
            }
        }


        protected override async Task Handle(SQSEvent sqsEvent, ILambdaContext context)
        {
            Logger.Info($"Processing Sqs event With {sqsEvent.Records?.Count} records.");

            if (sqsEvent?.Records == null) return;
            if (sqsEvent.Records.Count == 0) return;

            foreach (var record in sqsEvent.Records)
            {
                Logger.Info($"Processing SQS Event message ID {record.MessageId}.");

                var message = new QueueMessage<TBody>()
                {
                    MessageId = record.MessageId,
                    ReceiptHandle = record.ReceiptHandle,
                    Attributes = record.Attributes,
                    Body = Serializer.DeserializeObject<TBody>(record.Body)
                };
                
                message.ParseQueueAttributes(record.Attributes);

                await ProcessMessage(message);

                Logger.Info($"SQS Event message ID {record.MessageId} Processed.");
            }
        }

        protected abstract Task ProcessMessage(QueueMessage<TBody> message);
    }
}
