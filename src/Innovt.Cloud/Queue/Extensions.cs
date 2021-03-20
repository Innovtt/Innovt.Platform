using System.Collections.Generic;

namespace Innovt.Cloud.Queue
{
    public static class Extensions
    {
        public static void ParseQueueAttributes(this IQueueMessage queueMessage,
            Dictionary<string, string> queueAttributes)
        {
            if (queueAttributes == null)
                return;

            if (queueAttributes.ContainsKey("ApproximateReceiveCount"))
                queueMessage.ApproximateReceiveCount = int.Parse(queueAttributes["ApproximateReceiveCount"]);

            if (queueAttributes.ContainsKey("ApproximateFirstReceiveTimestamp"))
            {
                queueMessage.ApproximateFirstReceiveTimestamp =
                    double.Parse(queueAttributes["ApproximateFirstReceiveTimestamp"]);
            }
        }
    }
}