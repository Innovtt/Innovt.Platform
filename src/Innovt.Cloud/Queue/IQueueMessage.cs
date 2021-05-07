// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;

namespace Innovt.Cloud.Queue
{
    public interface IQueueMessage
    {
        string MessageId { get; set; }

        string ReceiptHandle { get; set; }
        
        double? ApproximateFirstReceiveTimestamp { get; set; }

        int? ApproximateReceiveCount { get; set; }

        Dictionary<string, string> Attributes { get; set; }
    }
}