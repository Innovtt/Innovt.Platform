// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Lambda.Sqs
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Innovt.Cloud.Queue;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Serialization;

namespace Innovt.Cloud.AWS.Lambda.Sqs
{
    public abstract class SqsEventProcessor<TBody> : EventProcessor<SQSEvent> where TBody : class
    {
        private ISerializer serializer;

        protected SqsEventProcessor(ILogger logger) : base(logger)
        {
        }

        protected SqsEventProcessor(ILogger logger, ISerializer serializer) : base(logger)
        {
            this.Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        protected SqsEventProcessor()
        {
        }

        private ISerializer Serializer
        {
            get { return serializer ??= new JsonSerializer(); }

            set => serializer = value;
        }


        protected override async Task Handle(SQSEvent message, ILambdaContext context)
        {
            Logger.Info($"Processing Sqs event With {message?.Records?.Count} records.");

            if (message?.Records == null) return;
            if (message.Records.Count == 0) return;

            using var activity = EventProcessorActivitySource.StartActivity(nameof(Handle));
            activity?.SetTag("SqsMessageRecordsCount", message?.Records?.Count);
                
            foreach (var record in message.Records)
            {
                Logger.Info($"Processing SQS Event message ID {record.MessageId}.");

                var queueMessage = new QueueMessage<TBody>
                {
                    MessageId = record.MessageId,
                    ReceiptHandle = record.ReceiptHandle,
                    Attributes = record.Attributes,
                    Body = Serializer.DeserializeObject<TBody>(record.Body)
                };

                if (record.Attributes is not null)
                {
                    queueMessage.ParseQueueAttributes(record.Attributes);

                    record.Attributes.TryGetValue("TraceId", out var traceId);

                    if (traceId is not null)
                    {
                        activity?.SetParentId(traceId);
                    }
                }

                activity?.SetTag("SqsMessageId", queueMessage.MessageId);
                activity?.SetTag("SqsMessageApproximateFirstReceiveTimestamp", queueMessage.ApproximateFirstReceiveTimestamp);
                activity?.SetTag("SqsMessageApproximateReceiveCount", queueMessage.ApproximateReceiveCount);                
                activity?.AddBaggage("Message.ElapsedTimeBeforeAttendedInMilliseconds", $"{DateTime.UtcNow.Subtract(TimeSpan.FromMilliseconds(queueMessage.ApproximateFirstReceiveTimestamp.GetValueOrDefault()).TotalMilliseconds}");
                activity?.AddBaggage("Message.ElapsedTimeBeforeAttendedInMinutes", $"{DateTime.UtcNow.Subtract(TimeSpan.FromMilliseconds(queueMessage.ApproximateFirstReceiveTimestamp.GetValueOrDefault()).TotalMilliseconds}");



                await ProcessMessage(queueMessage).ConfigureAwait(false);

                Logger.Info($"SQS Event message ID {record.MessageId} Processed.");
            }
        }

        protected abstract Task ProcessMessage(QueueMessage<TBody> message);
    }
}