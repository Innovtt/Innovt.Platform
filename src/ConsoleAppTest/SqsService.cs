using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.AWS.SQS;
using Innovt.Cloud.Queue;
using Innovt.Core.CrossCutting.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppTest
{
    public class Queue : IQueueMessage
    {
        public string MessageId { get; set; }
        public string ReceiptHandle { get; set; }
        public double? ApproximateFirstReceiveTimestamp { get; set; }
        public int? ApproximateReceiveCount { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }
    public class SqsService : QueueService<Queue>
    {
        public SqsService(ILogger logger) : base(logger)
        {
        }

        public SqsService(ILogger logger, string queueName) : base(logger, queueName)
        {
        }

        public SqsService(ILogger logger, IAWSConfiguration configuration) : base(logger, configuration)
        {
            //configuration.Profile
        }

        public SqsService(ILogger logger, IAWSConfiguration configuration, string queueName, string region = null) : base(logger, configuration, queueName, region)
        {
        }
    }
}
