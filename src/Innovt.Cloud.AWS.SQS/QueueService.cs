using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.Queue;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.SQS
{
    public class QueueService<T> :  AwsBaseService, IQueueService<T> where T:IQueueMessage
    {
        private AmazonSQSClient sqsClient = null;

        public string QueueName { get; protected set; }

        private static string queueUrl { get; set; }

        public QueueService(ILogger logger) : this(logger, typeof(T).Name)
        { 
        }

        public QueueService(ILogger logger, string queueName) : this(logger,null, queueName)
        {
        }

        public QueueService(ILogger logger, IAWSConfiguration configuration) : this(logger, configuration, typeof(T).Name)
        {   
         
        }

        public QueueService(ILogger logger, IAWSConfiguration configuration, string queueName, string region = null)
            : base(logger, configuration, region)
        {
            Check.NotNull(queueName, nameof(queueName));

            this.QueueName = queueName;
           
            sqsClient = CreateService<AmazonSQSClient>();
        }

        protected async Task<string> GetQueueUrlAsync()
        {
            if (queueUrl!=null && queueUrl.EndsWith(QueueName))
            {
                return queueUrl;
            }

            if (base.Configuration.AccountNumber!=null && base.Configuration.Region!=null)
            {
                queueUrl = $"https://sqs.{base.Configuration.Region}.amazonaws.com/{base.Configuration.AccountNumber}/{QueueName}";
            }
            else
            {
                queueUrl = (await sqsClient.GetQueueUrlAsync(QueueName))?.QueueUrl;
            }

            return queueUrl;
        }


        /// <summary>
        /// Enable user to receive messages
        /// </summary>
        /// <param name="quantity">1-10</param>
        /// <param name="waitTimeInSeconds">To enable long pooling</param>
        /// <param name="visibilityTimeoutInSeconds"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<T>> GetMessagesAsync(int quantity,int? waitTimeInSeconds=null, 
            int? visibilityTimeoutInSeconds=null,
            CancellationToken cancellationToken = default)
        {
            var request = new ReceiveMessageRequest()
            {  
                MaxNumberOfMessages = quantity, 
                AttributeNames = new List<string>() { "All" },
                QueueUrl = await GetQueueUrlAsync()
            };

            if (visibilityTimeoutInSeconds.HasValue)
                request.VisibilityTimeout = visibilityTimeoutInSeconds.Value;

            if (waitTimeInSeconds.HasValue)
                request.WaitTimeSeconds = waitTimeInSeconds.Value;

            var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async ()=> await sqsClient.ReceiveMessageAsync(request, cancellationToken));

            if (response.HttpStatusCode != HttpStatusCode.OK)
                throw new CriticalException("Error getting messages from the queue.");

            if (response.Messages == null)
                return null;

            var messages = new List<T>();

            foreach (var message in response.Messages)
            {   
                var tMessage = JsonSerializer.Deserialize<T>(message.Body);
                tMessage.MessageId = message.MessageId;
                tMessage.ReceiptHandle = message.ReceiptHandle;
                tMessage.Attributes = message.Attributes;
                messages.Add(tMessage);
            }
            return messages;
        }


        public async Task DeQueueAsync(string receiptHandle, CancellationToken cancellationToken = default)
        { 
            var queueUrl = await GetQueueUrlAsync();

            var deleteRequest = new DeleteMessageRequest(queueUrl, receiptHandle);

            await base.CreateDefaultRetryAsyncPolicy().ExecuteAndCaptureAsync(async () => await sqsClient.DeleteMessageAsync(deleteRequest, cancellationToken));
        }

        public async Task<int> ApproximateMessageCountAsync(CancellationToken cancellationToken = default)
        {
            var attributes = new List<string>() { "ApproximateNumberOfMessages" };
      
            var queueUrl = await GetQueueUrlAsync();

            var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async ()=> await sqsClient.GetQueueAttributesAsync(queueUrl, attributes, cancellationToken));

            return (response?.ApproximateNumberOfMessages).GetValueOrDefault();
        }

        public async Task CreateIfNotExistAsync(CancellationToken cancellationToken = default)
        {   
            await sqsClient.CreateQueueAsync(QueueName, cancellationToken);
        }
      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="delaySeconds"></param>
        /// <param name="cancellationToken">// Gets and sets the property DelaySeconds.
        ///  The length of time, in seconds, for which to delay a specific message. Valid values:
        /// 0 to 900. Maximum: 15 minutes. Messages with a positive <code>DelaySeconds</code>
        /// value become available for processing after the delay period is finished. If you don't
        /// specify a value, the default value for the queue applies.
        /// When you set <code>FifoQueue</code>, you can't set <code>DelaySeconds</code> per message.
        /// You can set this parameter only on a queue level.</param>
        /// <returns></returns>
        public async Task<string> QueueAsync(object message, int? delaySeconds = null, CancellationToken cancellationToken = default)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
           
            var messageRequest = new SendMessageRequest
            {
                MessageBody = JsonSerializer.Serialize(message),
                QueueUrl = await GetQueueUrlAsync()
        };

            if (delaySeconds.HasValue)
                messageRequest.DelaySeconds = delaySeconds.Value;

            var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async ()=> await sqsClient.SendMessageAsync(messageRequest, cancellationToken));

            if (response.HttpStatusCode != HttpStatusCode.OK)
                throw new CriticalException("Error sending message to queue.");

            return response.MessageId;
        }
    }
}