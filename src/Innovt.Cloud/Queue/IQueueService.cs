using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Cloud.Queue
{
    public interface IQueueService
    {
        IList<T> GetMessages<T>(int quantity, int? waitTimeInSeconds = null,
            int? visibilityTimeoutInSeconds = null)
            where T : IQueueMessage;

        Task<IList<T>> GetMessagesAsync<T>(int quantity, int? waitTimeInSeconds = null,
            int? visibilityTimeoutInSeconds = null, CancellationToken cancellationToken = default)
            where T : IQueueMessage;
        
        Task DeQueueAsync(string id, string popReceipt, CancellationToken cancellationToken = default);

        void DeQueue(string id, string popReceipt);

        Task QueueAsync(object message, int? visibilityTimeoutInSeconds = null, CancellationToken cancellationToken = default);
     
        void Queue(object message, int? visibilityTimeoutInSeconds = null);

        Task<int> ApproximateMessageCountAsync(CancellationToken cancellationToken = default);
        
        int ApproximateMessageCount();

        Task CreateIfNotExistAsync(CancellationToken cancellationToken = default);

        void CreateIfNotExist();
    }
}