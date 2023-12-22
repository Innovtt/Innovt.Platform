// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

namespace Innovt.Cloud.Queue;

/// <summary>
/// Represents the result of enqueuing a message into a message queue.
/// </summary>
public class MessageQueueResult
{
    /// <summary>
    /// Gets or sets the ID associated with the enqueued message.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets an error message in case of a failed enqueue operation.
    /// </summary>
    public string Error { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the enqueue operation was successful.
    /// </summary>
    public bool Success { get; set; }
}