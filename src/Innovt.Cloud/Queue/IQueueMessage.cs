// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System.Collections.Generic;

namespace Innovt.Cloud.Queue;

public interface IQueueMessage
{
    string MessageId { get; set; }

    string ReceiptHandle { get; set; }

    string TraceId { get; set; }

    double? ApproximateFirstReceiveTimestamp { get; set; }

    int? ApproximateReceiveCount { get; set; }

    Dictionary<string, string> Attributes { get; set; }
}