using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cloud.Queue
{
    public interface IQueueService<T> where T : IQueueMessage
    {
        Task<IList<T>> GetMessagesAsync(int quantity, int? waitTimeInSeconds = null,
            int? visibilityTimeoutInSeconds = null, CancellationToken cancellationToken = default);
            
        Task DeQueueAsync(string popReceipt, CancellationToken cancellationToken = default);

        Task<string> QueueAsync<K>(K message, int? visibilityTimeoutInSeconds = null, CancellationToken cancellationToken = default);

        Task<IList<MessageQueueResult>> QueueBatchAsync(IEnumerable<MessageBatchRequest> message, int? delaySeconds = null, CancellationToken cancellationToken = default);

        Task<int> ApproximateMessageCountAsync(CancellationToken cancellationToken = default);

        Task CreateIfNotExistAsync(CancellationToken cancellationToken = default);
    }
}