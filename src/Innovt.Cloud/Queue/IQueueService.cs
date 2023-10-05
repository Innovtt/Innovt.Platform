// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cloud.Queue;
/// <summary>
/// Defines the operations for interacting with a queue service for a specific type of queue messages.
/// </summary>
/// <typeparam name="T">The type of queue messages.</typeparam>
public interface IQueueService<T> where T : IQueueMessage
{
    /// <summary>
    /// Retrieves a list of messages from the queue asynchronously.
    /// </summary>
    /// <param name="quantity">The number of messages to retrieve.</param>
    /// <param name="waitTimeInSeconds">The maximum time to wait for messages to become available.</param>
    /// <param name="visibilityTimeoutInSeconds">The duration for which retrieved messages are invisible to other consumers.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A list of retrieved messages.</returns>
    Task<IList<T>> GetMessagesAsync(int quantity, int? waitTimeInSeconds = null,
        int? visibilityTimeoutInSeconds = null, CancellationToken cancellationToken = default);
    /// <summary>
    /// Removes a message from the queue asynchronously.
    /// </summary>
    /// <param name="popReceipt">The pop receipt of the message to be removed.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeQueueAsync(string popReceipt, CancellationToken cancellationToken = default);
    /// <summary>
    /// Enqueues a message asynchronously.
    /// </summary>
    /// <typeparam name="TK">The type of the message to be enqueued.</typeparam>
    /// <param name="message">The message to be enqueued.</param>
    /// <param name="visibilityTimeoutInSeconds">The duration for which the enqueued message is invisible to other consumers.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The message ID associated with the enqueued message.</returns>
    Task<string> EnQueueAsync<TK>(TK message, int? visibilityTimeoutInSeconds = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Enqueues a batch of messages asynchronously.
    /// </summary>
    /// <param name="message">The collection of messages to be enqueued.</param>
    /// <param name="delaySeconds">The delay in seconds before the messages become available for retrieval.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A list of message enqueue results.</returns>
    Task<IList<MessageQueueResult>> EnQueueBatchAsync(IEnumerable<MessageBatchRequest> message,
        int? delaySeconds = null, CancellationToken cancellationToken = default);
    /// <summary>
    /// Retrieves the approximate number of messages in the queue asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The approximate message count in the queue.</returns>
    Task<int> ApproximateMessageCountAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Creates the queue if it does not already exist asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateIfNotExistAsync(CancellationToken cancellationToken = default);
}