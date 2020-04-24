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
    public class QueueService :  AWSBaseService, IQueueService
    { 
        public string QueueName { get; private set; }

        protected async Task<string> GetQueueUrlAsync()
        {
            using var s3Client = CreateService<AmazonSQSClient>();

            return (await s3Client.GetQueueUrlAsync(QueueName))?.QueueUrl;
        }

        public QueueService(ILogger logger, string queueName) : base(logger)
        {
            Check.NotNull(queueName, nameof(queueName));
            this.QueueName = queueName;
        }

        public QueueService(IAWSConfiguration configuration,ILogger logger, string queueName, string region=null) 
            : base(configuration, logger,region)
        {
            Check.NotNull(queueName, nameof(queueName));
            this.QueueName = queueName;
        }

        /// <summary>
        /// Enable user to receive messages
        /// </summary>
        /// <param name="quantity">1-10</param>
        /// <param name="waitTimeInSeconds">To enable long pooling</param>
        /// <param name="visibilityTimeoutInSeconds"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<T>> GetMessagesAsync<T>(int quantity,int? waitTimeInSeconds=null, 
            int? visibilityTimeoutInSeconds=null,
            CancellationToken cancellationToken = default) where T: IQueueMessage
        {
            var request = new ReceiveMessageRequest()
            {  
                MaxNumberOfMessages = quantity, 
                AttributeNames = new List<string>() { "All" } 
            };

            if (visibilityTimeoutInSeconds.HasValue)
                request.VisibilityTimeout = visibilityTimeoutInSeconds.Value;

            if (waitTimeInSeconds.HasValue)
                request.WaitTimeSeconds = waitTimeInSeconds.Value;
         
            using var s3Client  =  CreateService<AmazonSQSClient>();

            request.QueueUrl = (await s3Client.GetQueueUrlAsync(QueueName, cancellationToken)).QueueUrl;

            var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async ()=> await s3Client.ReceiveMessageAsync(request, cancellationToken));

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

            /// <returns></returns>
        public IList<T> GetMessages<T>(int quantity,int? waitTimeInSeconds=null, int? visibilityTimeoutInSeconds=null) where T: IQueueMessage{

           return AsyncHelper.RunSync(async () => await GetMessagesAsync<T>(quantity, waitTimeInSeconds, visibilityTimeoutInSeconds));
        }

        public async Task DeQueueAsync(string id, string receiptHandle, CancellationToken cancellationToken = default)
        { 
            using var s3Client  =  CreateService<AmazonSQSClient>();

            var queueUrl = await GetQueueUrlAsync();

            var deleteRequest = new DeleteMessageRequest(queueUrl, receiptHandle);

            await base.CreateDefaultRetryAsyncPolicy().ExecuteAndCaptureAsync(async () => await s3Client.DeleteMessageAsync(deleteRequest, cancellationToken));
        }

        public void DeQueue(string id, string receiptHandle)
        {
            AsyncHelper.RunSync(async () => await DeQueueAsync(id,receiptHandle));
        }

        public async Task<int> ApproximateMessageCountAsync(CancellationToken cancellationToken = default)
        {
            var attributes = new List<string>() { "ApproximateNumberOfMessages" };
            
            using var s3Client  =  CreateService<AmazonSQSClient>();

            var queueUrl = await GetQueueUrlAsync();

            var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async ()=> await s3Client.GetQueueAttributesAsync(queueUrl, attributes, cancellationToken));

            return (response?.ApproximateNumberOfMessages).GetValueOrDefault();
        }

        public int ApproximateMessageCount()
        {
             return AsyncHelper.RunSync(async () => await ApproximateMessageCountAsync());
        }

        public async Task CreateIfNotExistAsync(CancellationToken cancellationToken = default)
        {   
            using var s3Client  =  CreateService<AmazonSQSClient>();

            await s3Client.CreateQueueAsync(QueueName, cancellationToken);
        }

        public void CreateIfNotExist()
        {   
           AsyncHelper.RunSync(async () => await CreateIfNotExistAsync());
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
        public async Task QueueAsync(object message, int? delaySeconds = null, CancellationToken cancellationToken = default)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
           
            var messageRequest = new SendMessageRequest
            {
                MessageBody = JsonSerializer.Serialize(message),
                QueueUrl = await GetQueueUrlAsync()
        };

            if (delaySeconds.HasValue)
                messageRequest.DelaySeconds = delaySeconds.Value;
           
            using var s3Client  =  CreateService<AmazonSQSClient>();

            var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async ()=> await s3Client.SendMessageAsync(messageRequest, cancellationToken));

            if (response.HttpStatusCode != HttpStatusCode.OK)
                throw new CriticalException("Error sending message to queue.");
        }

        public void Queue(object message, int? delaySeconds = null)
        {
            AsyncHelper.RunSync(async () => await QueueAsync(message,delaySeconds));
        }
    }
}