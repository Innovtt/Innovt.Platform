using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.SQS;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.AWS.Dynamo;
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
    public class SqsService : Repository
    {
        public SqsService(ILogger logger, IAWSConfiguration configuration) : base(logger, configuration)
        {
        }

        public SqsService(ILogger logger, IAWSConfiguration configuration, string region) : base(logger, configuration, region)
        {
        }
    }
}
