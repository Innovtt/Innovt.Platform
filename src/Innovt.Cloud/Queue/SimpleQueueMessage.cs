// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

namespace Innovt.Cloud.Queue;

//You Can use it to abstract infrastructure implementations
public class SimpleQueueMessage<T>
{
    public SimpleQueueMessage()
    {
    }

    public SimpleQueueMessage(T value)
    {
        Value = value;
    }

    public T Value { get; set; }
}