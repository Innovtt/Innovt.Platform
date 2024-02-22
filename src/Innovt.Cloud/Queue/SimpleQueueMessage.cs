// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

namespace Innovt.Cloud.Queue;

//You Can use it to abstract infrastructure implementations
/// <summary>
///     Represents a simple queue message.
/// </summary>
/// <typeparam name="T">The type of value held in the message.</typeparam>
public class SimpleQueueMessage<T>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SimpleQueueMessage{T}" /> class.
    /// </summary>
    public SimpleQueueMessage()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SimpleQueueMessage{T}" /> class with a specified value.
    /// </summary>
    /// <param name="value">The value to be set for the message.</param>
    public SimpleQueueMessage(T value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets or sets the value of the queue message.
    /// </summary>
    public T Value { get; set; }
}