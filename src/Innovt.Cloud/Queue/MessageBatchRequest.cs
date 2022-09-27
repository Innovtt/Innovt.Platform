// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

namespace Innovt.Cloud.Queue;

public class MessageBatchRequest
{
    public string Id { get; set; }

    public object Message { get; set; }
}