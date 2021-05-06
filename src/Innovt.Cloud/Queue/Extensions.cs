// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

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
                queueMessage.ApproximateFirstReceiveTimestamp =
                    double.Parse(queueAttributes["ApproximateFirstReceiveTimestamp"]);
        }
    }
}