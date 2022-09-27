// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

namespace Innovt.Cloud.Queue;

public class MessageQueueResult
{
    public string Id { get; set; }

    public string Error { get; set; }

    public bool Success { get; set; }
}