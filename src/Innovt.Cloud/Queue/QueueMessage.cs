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

        public QueueMessage(Dictionary<string, string> attributes)
        {
            Attributes = attributes;
            this.ParseQueueAttributes(attributes);
        }
        public void ParseQueueAttributes(Dictionary<string, string> queueAttributes)
        {
            if (queueAttributes == null)
                return;

            if (queueAttributes.ContainsKey("ApproximateReceiveCount"))
                this.ApproximateReceiveCount = int.Parse(queueAttributes["ApproximateReceiveCount"]);

            if (queueAttributes.ContainsKey("ApproximateFirstReceiveTimestamp"))
            {
                this.ApproximateFirstReceiveTimestamp =
                    double.Parse(queueAttributes["ApproximateFirstReceiveTimestamp"]);
            }
        }
    }
}