// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System.Collections.Generic;

namespace Innovt.Cloud.Queue;
/// <summary>
/// Represents a message in a queue.
/// </summary>
public interface IQueueMessage
{
    /// <summary>
    /// Gets or sets the unique identifier of the message.
    /// </summary>
    string MessageId { get; set; }
    /// <summary>
    /// Gets or sets the receipt handle for the message.
    /// </summary>
    string ReceiptHandle { get; set; }
    /// <summary>
    /// Gets or sets the trace ID associated with the message.
    /// </summary>
    string TraceId { get; set; }
    /// <summary>
    /// Gets or sets the approximate timestamp when the message was first received.
    /// </summary>
    double? ApproximateFirstReceiveTimestamp { get; set; }
    /// <summary>
    /// Gets or sets the approximate number of times the message has been received.
    /// </summary>
    int? ApproximateReceiveCount { get; set; }
    /// <summary>
    /// Gets or sets the attributes associated with the message in the form of key-value pairs.
    /// </summary>
    Dictionary<string, string> Attributes { get; set; }
}