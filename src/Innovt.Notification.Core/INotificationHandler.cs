using System.Threading;
using System.Threading.Tasks;
using Innovt.Notification.Core.Domain;

namespace Innovt.Notification.Core
{
    public interface INotificationHandler
    {
        Task<dynamic> SendAsync(NotificationMessage message, CancellationToken cancellationToken = default);
    }
}