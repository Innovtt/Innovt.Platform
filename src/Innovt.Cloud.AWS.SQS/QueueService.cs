using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Cloud.Queue;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Exceptions;
using Innovt.Core.Serialization;

namespace Innovt.Cloud.AWS.SQS
{
    public class QueueService<T> : AwsBaseService, IQueueService<T> where T : IQueueMessage
    {
        public string QueueName { get; protected set; }
        public static string QueueUrl { get; private set; }

        private ISerializer serializer;

        public QueueService(ILogger logger, IAWSConfiguration configuration, string queueName = null,
            ISerializer serializer = null) : base(logger, configuration)
        {
            this.serializer = serializer;
            QueueName = queueName ?? typeof(T).Name;
        }

        public QueueService(ILogger logger, IAWSConfiguration configuration, string region, string queueName = null,
            ISerializer serializer = null) : base(logger, configuration, region)
        {
            this.serializer = serializer;
            QueueName = queueName ?? typeof(T).Name;
        }

        private AmazonSQSClient sqsClient = null;

        private AmazonSQSClient SqsClient => sqsClient ??= CreateService<AmazonSQSClient>();

        private async Task<string> GetQueueUrlAsync()
        {
            if (QueueUrl != null && QueueUrl.EndsWith(QueueName)) return QueueUrl;

            if (Configuration?.AccountNumber != null)
                QueueUrl =
                    $"https://sqs.{GetServiceRegionEndPoint().SystemName}.amazonaws.com/{Configuration.AccountNumber}/{QueueName}";
            else
                QueueUrl = (await SqsClient.GetQueueUrlAsync(QueueName))?.QueueUrl;

            return QueueUrl;
        }


        private ISerializer Serializer => serializer ??= new JsonSerializer();

        /// <summary>
        /// Enable user to receive messages
        /// </summary>
        /// <param name="quantity">1-10</param>
        /// <param name="waitTimeInSeconds">To enable long pooling</param>
        /// <param name="visibilityTimeoutInSeconds"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<T>> GetMessagesAsync(int quantity, int? waitTimeInSeconds = null,
            int? visibilityTimeoutInSeconds = null,
            CancellationToken cancellationToken = default)
        {
            var request = new ReceiveMessageRequest()
            {
                MaxNumberOfMessages = quantity,
                AttributeNames = new List<string>() {"All"},
                QueueUrl = await GetQueueUrlAsync()
            };

            if (visibilityTimeoutInSeconds.HasValue)
                request.VisibilityTimeout = visibilityTimeoutInSeconds.Value;

            if (waitTimeInSeconds.HasValue)
                request.WaitTimeSeconds = waitTimeInSeconds.Value;

            var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await SqsClient.ReceiveMessageAsync(request, cancellationToken));

            if (response.HttpStatusCode != HttpStatusCode.OK)
                throw new CriticalException("Error getting messages from the queue.");

            if (response.Messages == null)
                return null;

            var messages = new List<T>();

            foreach (var message in response.Messages)
            {
                var tMessage = Serializer.DeserializeObject<T>(message.Body);
                tMessage.MessageId = message.MessageId;
                tMessage.ReceiptHandle = message.ReceiptHandle;
                tMessage.Attributes = message.Attributes;
                tMessage.ParseQueueAttributes(message.Attributes);
                messages.Add(tMessage);
            }

            return messages;
        }


        public async Task DeQueueAsync(string receiptHandle, CancellationToken cancellationToken = default)
        {
            var queueUrl = await GetQueueUrlAsync();

            var deleteRequest = new DeleteMessageRequest(queueUrl, receiptHandle);

            await base.CreateDefaultRetryAsyncPolicy().ExecuteAndCaptureAsync(async () =>
                await SqsClient.DeleteMessageAsync(deleteRequest, cancellationToken));
        }

        public async Task<int> ApproximateMessageCountAsync(CancellationToken cancellationToken = default)
        {
            var attributes = new List<string>() {"ApproximateNumberOfMessages"};

            var queueUrl = await GetQueueUrlAsync();

            var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await SqsClient.GetQueueAttributesAsync(queueUrl, attributes, cancellationToken));

            return (response?.ApproximateNumberOfMessages).GetValueOrDefault();
        }

        public async Task CreateIfNotExistAsync(CancellationToken cancellationToken = default)
        {
            await SqsClient.CreateQueueAsync(QueueName, cancellationToken);
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
        public async Task<string> QueueAsync<K>(K message, int? delaySeconds = null,
            CancellationToken cancellationToken = default)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            var messageRequest = new SendMessageRequest
            {
                MessageBody = Serializer.SerializeObject(message),
                QueueUrl = await GetQueueUrlAsync()
            };

            if (delaySeconds.HasValue)
                messageRequest.DelaySeconds = delaySeconds.Value;

            var response = await base.CreateDefaultRetryAsyncPolicy()
                .ExecuteAsync(async () => await SqsClient.SendMessageAsync(messageRequest, cancellationToken))
                .ConfigureAwait(false);

            if (response.HttpStatusCode != HttpStatusCode.OK)
                throw new CriticalException("Error sending message to queue.");


            return response.MessageId;
        }

        public async Task<IList<MessageQueueResult>> QueueBatchAsync(IEnumerable<MessageBatchRequest> message,
            int? delaySeconds = null, CancellationToken cancellationToken = default)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            var messageRequest = new SendMessageBatchRequest
            {
                QueueUrl = await GetQueueUrlAsync()
            };

            messageRequest.Entries = new List<SendMessageBatchRequestEntry>();
            foreach (var item in message)
                messageRequest.Entries.Add(new SendMessageBatchRequestEntry()
                {
                    Id = item.Id,
                    DelaySeconds = delaySeconds.GetValueOrDefault(),
                    MessageBody = Serializer.SerializeObject(item.Message)
                });

            var response = await base.CreateDefaultRetryAsyncPolicy()
                .ExecuteAsync(async () => await SqsClient.SendMessageBatchAsync(messageRequest, cancellationToken))
                .ConfigureAwait(false);

            var result = new List<MessageQueueResult>();

            if (response.Successful != null)
                foreach (var item in response.Successful)
                    result.Add(new MessageQueueResult() {Id = item.Id, Success = true});

            if (response.Failed != null)
                foreach (var item in response.Failed)
                    result.Add(new MessageQueueResult() {Id = item.Id, Success = false, Error = item.Message});

            return result;
        }

        protected override void DisposeServices()
        {
            sqsClient?.Dispose();
        }
    }
}