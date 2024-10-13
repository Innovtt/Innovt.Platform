// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.SQS

using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Innovt.Cloud.AWS.SQS;

/// <summary>
///     Provides functionalities to interact with an Amazon SQS queue for a specified message type
///     <typeparamref name="T" />.
/// </summary>
/// <typeparam name="T">Type of the messages in the queue, must implement <see cref="IQueueMessage" />.</typeparam>
public class QueueService<T> : AwsBaseService, IQueueService<T> where T : IQueueMessage
{
    private static readonly ActivitySource QueueActivitySource = new("Innovt.Cloud.AWS.SQS.QueueService");

    private ISerializer serializer;

    private AmazonSQSClient sqsClient;

    /// <summary>
    ///     Initializes a new instance of the <see cref="QueueService{T}" /> class.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="configuration">AWS configuration.</param>
    /// <param name="queueName">Optional queue name, defaults to the name of type <typeparamref name="T" />.</param>
    /// <param name="serializer">Custom serializer (optional).</param>
    public QueueService(ILogger logger, IAwsConfiguration configuration, string queueName = null,
        ISerializer serializer = null) : base(logger, configuration)
    {
        this.serializer = serializer;
        QueueName = queueName ?? typeof(T).Name;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="QueueService{T}" /> class.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="configuration">AWS configuration.</param>
    /// <param name="queueName">Optional queue name, defaults to the name of type <typeparamref name="T" />.</param>
    /// <param name="serializer">Custom serializer (optional).</param>
    public QueueService(ILogger logger, IAwsConfiguration configuration, string region, string queueName = null,
        ISerializer serializer = null) : base(logger, configuration, region)
    {
        this.serializer = serializer;
        QueueName = queueName ?? typeof(T).Name;
    }

    /// <summary>
    ///     Gets the name of the queue.
    /// </summary>
    public string QueueName { get; protected set; }

    /// <summary>
    ///     Gets the URL of the queue.
    /// </summary>
#pragma warning disable CA1056 // URI-like properties should not be strings
    public string QueueUrl { get; private set; }
#pragma warning restore CA1056 // URI-like properties should not be strings
    /// <summary>
    ///     Gets the Amazon SQS client for queue operations.
    /// </summary>
    private AmazonSQSClient SqsClient => sqsClient ??= CreateService<AmazonSQSClient>();

    /// <summary>
    ///     Gets the serializer for message serialization/deserialization.
    /// </summary>
    private ISerializer Serializer => serializer ??= new JsonSerializer();

    /// <summary>
    ///     Enable user to receive messages
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
        using var activity = QueueActivitySource.StartActivity();
        activity?.SetTag("sqs.quantity", quantity);
        activity?.SetTag("sqs.waitTimeInSeconds", waitTimeInSeconds);
        activity?.SetTag("sqs.visibilityTimeoutInSeconds", visibilityTimeoutInSeconds);

        var request = new ReceiveMessageRequest
        {
            MaxNumberOfMessages = quantity,
            MessageSystemAttributeNames = ["All"],
            QueueUrl = await GetQueueUrlAsync().ConfigureAwait(false)
        };

        if (visibilityTimeoutInSeconds.HasValue)
            request.VisibilityTimeout = visibilityTimeoutInSeconds.Value;

        if (waitTimeInSeconds.HasValue)
            request.WaitTimeSeconds = waitTimeInSeconds.Value;

        var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await SqsClient.ReceiveMessageAsync(request, cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);

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


    /// <summary>
    ///     Dequeues a message asynchronously from the queue using the provided pop receipt.
    /// </summary>
    /// <param name="popReceipt">The pop receipt of the message to be dequeued.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task DeQueueAsync(string popReceipt, CancellationToken cancellationToken = default)
    {
        using var activity = QueueActivitySource.StartActivity();
        activity?.SetTag("sqs.receipt_handle", popReceipt);

        var queueUrl = await GetQueueUrlAsync().ConfigureAwait(false);

        var deleteRequest = new DeleteMessageRequest(queueUrl, popReceipt);

        await base.CreateDefaultRetryAsyncPolicy().ExecuteAndCaptureAsync(async () =>
                await SqsClient.DeleteMessageAsync(deleteRequest, cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    /// <summary>
    ///     Retrieves the approximate message count of the queue asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>The approximate message count of the queue.</returns>
    public async Task<int> ApproximateMessageCountAsync(CancellationToken cancellationToken = default)
    {
        using var activity = QueueActivitySource.StartActivity();

        var attributes = new List<string> { "ApproximateNumberOfMessages" };

        var queueUrl = await GetQueueUrlAsync().ConfigureAwait(false);

        var response = await base.CreateDefaultRetryAsyncPolicy().ExecuteAsync(async () =>
                await SqsClient.GetQueueAttributesAsync(queueUrl, attributes, cancellationToken)
                    .ConfigureAwait(false))
            .ConfigureAwait(false);


        activity?.SetTag("sqs.approximate_number_of_messages", response?.ApproximateNumberOfMessages);

        return (response?.ApproximateNumberOfMessages).GetValueOrDefault();
    }

    /// <summary>
    ///     Creates the queue if it does not exist asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CreateIfNotExistAsync(CancellationToken cancellationToken = default)
    {
        using var activity = QueueActivitySource.StartActivity();
        activity?.SetTag("sqs.queue_name", QueueName);

        await SqsClient.CreateQueueAsync(QueueName, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// </summary>
    /// <param name="message"></param>
    /// <param name="visibilityTimeoutInSeconds"></param>
    /// <param name="cancellationToken">
    ///     // Gets and sets the property DelaySeconds.
    ///     The length of time, in seconds, for which to delay a specific message. Valid values:
    ///     0 to 900. Maximum: 15 minutes. Messages with a positive <code>DelaySeconds</code>
    ///     value become available for processing after the delay period is finished. If you don't
    ///     specify a value, the default value for the queue applies.
    ///     When you set <code>FifoQueue</code>, you can't set <code>DelaySeconds</code> per message.
    ///     You can set this parameter only on a queue level.
    /// </param>
    /// <returns></returns>
    public async Task<string> EnQueueAsync<TK>(TK message, int? visibilityTimeoutInSeconds = null,
        CancellationToken cancellationToken = default)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));

        using var activity = QueueActivitySource.StartActivity("QueueAsync");
        activity?.SetTag("sqs.delay_seconds", visibilityTimeoutInSeconds);

        var messageRequest = new SendMessageRequest
        {
            MessageBody = Serializer.SerializeObject(message),
            QueueUrl = await GetQueueUrlAsync().ConfigureAwait(false)
        };

        activity?.SetTag("sqs.queue_url", messageRequest.QueueUrl);

        EnrichMessage(activity, messageRequest);

        if (visibilityTimeoutInSeconds.HasValue)
            messageRequest.DelaySeconds = visibilityTimeoutInSeconds.Value;

        var response = await base.CreateDefaultRetryAsyncPolicy()
            .ExecuteAsync(async () =>
                await SqsClient.SendMessageAsync(messageRequest, cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);

        activity?.SetTag("sqs.status_code", response.HttpStatusCode);

        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new CriticalException("Error sending message to queue.");

        return response.MessageId;
    }

    /// <summary>
    ///     Enqueues a batch of messages asynchronously to the queue.
    /// </summary>
    /// <param name="message">The messages to be enqueued.</param>
    /// <param name="delaySeconds">The delay in seconds for message delivery (optional).</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>A list of message queue results indicating success or failure for each message.</returns>
    public async Task<IList<MessageQueueResult>> EnQueueBatchAsync(IEnumerable<MessageBatchRequest> message,
        int? delaySeconds = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        using var activity = QueueActivitySource.StartActivity();
        activity?.SetTag("sqs.delay_seconds", delaySeconds);

        var messageRequest = new SendMessageBatchRequest
        {
            QueueUrl = await GetQueueUrlAsync().ConfigureAwait(false),
            Entries = new List<SendMessageBatchRequestEntry>()
        };

        foreach (var item in message)
            messageRequest.Entries.Add(new SendMessageBatchRequestEntry
            {
                Id = item.Id,
                DelaySeconds = delaySeconds.GetValueOrDefault(),
                MessageBody = Serializer.SerializeObject(item.Message)
            });

        var response = await base.CreateDefaultRetryAsyncPolicy()
            .ExecuteAsync(async () =>
                await SqsClient.SendMessageBatchAsync(messageRequest, cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);

        var result = new List<MessageQueueResult>();

        activity?.SetTag("sqs.status_code", response.HttpStatusCode);
        activity?.SetTag("sqs.status_code", response.Failed);


        if (response.Successful != null)
            foreach (var item in response.Successful)
                result.Add(new MessageQueueResult { Id = item.Id, Success = true });

        if (response.Failed == null) return result;

        foreach (var item in response.Failed)
        {
            result.Add(new MessageQueueResult { Id = item.Id, Success = false, Error = item.Message });
            activity?.SetTag($"sqs.message_{item.Id}_id", item.Id);
            activity?.SetTag($"sqs.message_{item.Id}_message", item.Message);
            activity?.SetTag($"sqs.message_{item.Id}_code", item.Code);
            activity?.SetTag($"sqs.message_{item.Id}_sender_fault", item.SenderFault);
        }


        return result;
    }

    /// <summary>
    ///     Enriches the message request with additional attributes based on the activity.
    /// </summary>
    /// <param name="activity">The activity associated with the message.</param>
    /// <param name="messageRequest">The message request to be enriched.</param>
    private static void EnrichMessage(Activity activity, SendMessageRequest messageRequest)
    {
        if (activity == null) return;

        if (!string.IsNullOrEmpty(activity.ParentId))
            messageRequest.MessageAttributes.TryAdd("ParentId", new MessageAttributeValue
            {
                StringValue = activity.ParentId
            });

        if (string.IsNullOrEmpty(activity.RootId))
            messageRequest.MessageAttributes.TryAdd("RootTraceId", new MessageAttributeValue
            {
                StringValue = activity.RootId
            });
    }

    /// <summary>
    ///     Gets the queue URL asynchronously based on the queue name.
    /// </summary>
    /// <returns>The URL of the queue.</returns>
    private async Task<string> GetQueueUrlAsync()
    {
        if (QueueUrl != null && QueueUrl.EndsWith(QueueName, StringComparison.OrdinalIgnoreCase)) return QueueUrl;

        using var activity = QueueActivitySource.StartActivity();
        activity?.SetTag("sqs.account_number", Configuration?.AccountNumber);
        activity?.SetTag("sqs.queue_name", QueueName);

        QueueUrl = Configuration?.AccountNumber != null
            ? $"https://sqs.{GetServiceRegionEndPoint().SystemName}.amazonaws.com/{Configuration.AccountNumber}/{QueueName}"
            : (await SqsClient.GetQueueUrlAsync(QueueName).ConfigureAwait(false))?.QueueUrl;

        activity?.SetTag("sqs.queue_url", QueueUrl);

        return QueueUrl;
    }

    /// <summary>
    ///     Disposes of the Amazon SQS client.
    /// </summary>
    protected override void DisposeServices()
    {
        sqsClient?.Dispose();
    }
}