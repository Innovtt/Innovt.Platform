// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System.Collections.Generic;

namespace Innovt.Cloud.Queue;

/// <summary>
///     Represents a message in a queue with a specified body type.
/// </summary>
/// <typeparam name="T">The type of the message body.</typeparam>
public class QueueMessage<T> : IQueueMessage
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="QueueMessage{T}" /> class.
    /// </summary>
    public QueueMessage()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="QueueMessage{T}" /> class with a specified body.
    /// </summary>
    /// <param name="body">The body of the message.</param>
    public QueueMessage(T body)
    {
        Body = body;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="QueueMessage{T}" /> class with specified attributes.
    /// </summary>
    /// <param name="attributes">The attributes associated with the message.</param>
    public QueueMessage(Dictionary<string, string> attributes)
    {
        Attributes = attributes;
    }

    /// <summary>
    ///     Gets or sets the body of the message.
    /// </summary>
    public T Body { get; set; }

    /// <summary>
    ///     Gets or sets the ID associated with the message.
    /// </summary>
    public string MessageId { get; set; }

    /// <summary>
    ///     Gets or sets the receipt handle for the message.
    /// </summary>
    public string ReceiptHandle { get; set; }

    /// <summary>
    ///     Gets or sets the trace ID associated with the message.
    /// </summary>
    public string TraceId { get; set; }

    /// <summary>
    ///     Gets or sets the approximate timestamp when the message was first received.
    /// </summary>
    public double? ApproximateFirstReceiveTimestamp { get; set; }

    /// <summary>
    ///     Gets or sets the approximate number of times the message has been received.
    /// </summary>
    public int? ApproximateReceiveCount { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
    /// <summary>
    ///     Gets or sets the attributes associated with the message in the form of key-value pairs.
    /// </summary>
    public Dictionary<string, string> Attributes { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
}