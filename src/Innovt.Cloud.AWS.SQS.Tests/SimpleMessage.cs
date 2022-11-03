// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.SQS.Tests

using System.Collections.Generic;
using Innovt.Cloud.Queue;

namespace Innovt.Cloud.AWS.SQS.Tests;

public class SimpleMessage: IQueueMessage
{
    public string MessageId { get; set; }
    public string ReceiptHandle { get; set; }
    public string TraceId { get; set; }
    public double? ApproximateFirstReceiveTimestamp { get; set; }
    public int? ApproximateReceiveCount { get; set; }
    public Dictionary<string, string> Attributes { get; set; }
}