using System.Collections.Generic;

namespace Innovt.Cloud.Queue
{
    public class QueueMessage<T> : IQueueMessage
    {
        public string MessageId { get; set; }

        public string ReceiptHandle { get; set; }

        public T Body { get; set; }

        public double? ApproximateFirstReceiveTimestamp { get; set; }

        public int? ApproximateReceiveCount { get; set; }

        public Dictionary<string, string> Attributes { get; set; }

        public QueueMessage()
        {
        }

        public QueueMessage(T body)
        {
            Body = body;
        }

        public QueueMessage(Dictionary<string, string> attributes)
        {
            this.Attributes = attributes;
        }
    }
}