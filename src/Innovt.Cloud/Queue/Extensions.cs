// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;
using System.Globalization;

namespace Innovt.Cloud.Queue;

public static class QueueExtensions
{
    public static void ParseQueueAttributes(this IQueueMessage queueMessage,
        Dictionary<string, string> queueAttributes)
    {
        if (queueMessage is null || queueAttributes == null)
            return;

        if (queueAttributes.ContainsKey("ApproximateReceiveCount"))
            queueMessage.ApproximateReceiveCount = int.Parse(queueAttributes["ApproximateReceiveCount"],
                NumberStyles.Integer, CultureInfo.InvariantCulture);

        if (queueAttributes.ContainsKey("ApproximateFirstReceiveTimestamp"))
            queueMessage.ApproximateFirstReceiveTimestamp =
                double.Parse(queueAttributes["ApproximateFirstReceiveTimestamp"], NumberStyles.Number,
                    CultureInfo.InvariantCulture);
    }
}