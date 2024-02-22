// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System.Collections.Generic;
using System.Globalization;

namespace Innovt.Cloud.Queue;

/// <summary>
///     Extension methods for parsing queue message attributes.
/// </summary>
public static class QueueExtensions
{
    /// <summary>
    ///     Parses queue message attributes and updates the provided <paramref name="queueMessage" />.
    /// </summary>
    /// <param name="queueMessage">The queue message.</param>
    /// <param name="queueAttributes">The queue attributes to parse.</param>
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