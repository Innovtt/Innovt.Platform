namespace Innovt.Cloud.Queue;

//You Can use it to abstract infrastructure implementations
public class SimpleQueueMessage<T>
{
    public T Value { get; set; }

    public SimpleQueueMessage()
    {
    }

    public SimpleQueueMessage(T value)
    {
        Value = value;
    }
}





