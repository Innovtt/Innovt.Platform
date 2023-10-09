// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

namespace Innovt.Cloud.Queue;
/// <summary>
/// Represents a message and its associated ID in a batch for enqueueing.
/// </summary>
public class MessageBatchRequest
{
    /// <summary>
    /// Gets or sets the ID associated with the message.
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Gets or sets the message to be enqueued.
    /// </summary>
    public object Message { get; set; }
}