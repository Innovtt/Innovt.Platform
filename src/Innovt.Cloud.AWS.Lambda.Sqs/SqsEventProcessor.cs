// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Lambda.Sqs
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Innovt.Cloud.Queue;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Serialization;
using Innovt.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Innovt.Cloud.AWS.Lambda.Sqs
{
    /// <summary>
    /// If you're using this feature with a FIFO queue, your function should stop processing messages after the first failure and return all failed and unprocessed messages in batchItemFailures. This helps preserve the ordering of messages in your queue.
    /// </summary>
    /// <typeparam name="TBody"></typeparam>
    public abstract class SqsEventProcessor<TBody> : EventProcessor<SQSEvent, BatchFailureResponse> where TBody : class
    {
        private ISerializer serializer;
        private readonly bool isFifo;

        protected SqsEventProcessor(ILogger logger, bool isFifo = false) : base(logger)
        {
            this.isFifo = isFifo;
        }

        protected SqsEventProcessor(ILogger logger, ISerializer serializer, bool isFifo = false) : base(logger)
        {
            this.Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            this.isFifo = isFifo;
        }

        protected SqsEventProcessor(bool isFifo = false)
        {
            this.isFifo = isFifo;
        }

        private ISerializer Serializer
        {
            get { return serializer ??= new JsonSerializer(); }

            set => serializer = value;
        }

        protected override async Task<BatchFailureResponse> Handle(SQSEvent message, ILambdaContext context)
        {
            Logger.Info($"Processing Sqs event With {message?.Records?.Count} records.");

            using var watcher = new StopWatchHelper(Logger, nameof(Handle));

            var response = new BatchFailureResponse();

            if (message?.Records == null || message.Records.Count == 0) return response;

            using var activity = EventProcessorActivitySource.StartActivity(nameof(Handle));
            activity?.SetTag("SqsMessageRecordsCount", message?.Records?.Count);

            var processedMessages = new List<string>();

            foreach (var record in message.Records)
            {
                try
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
                    activity?.AddBaggage("Message.ElapsedTimeBeforeAttendedInMilliseconds", $"{queueMessage.ApproximateFirstReceiveTimestamp.GetValueOrDefault()}");

                    await ProcessMessage(queueMessage).ConfigureAwait(false);

                    processedMessages.Add(record.MessageId);

                    Logger.Info($"SQS Event message ID {record.MessageId} Processed.");
                }
                catch
                {
                    Logger.Warning($"SQS Event message ID {record.MessageId} will be returned as item failure.");

                    if (isFifo)
                    {
                        response.AddItems(GetRemainingMessages(message, processedMessages));
                        break;
                    }

                    response.AddItem(record.MessageId);
                }
            }

            return response;
        }

        private static IEnumerable<string> GetRemainingMessages(SQSEvent message, IList<string> processedMessages)
        {
            return message.Records.Where(r => !processedMessages.Contains(r.MessageId)).Distinct().Select(r => r.MessageId);
        }

        protected abstract Task ProcessMessage(QueueMessage<TBody> message);
    }
}