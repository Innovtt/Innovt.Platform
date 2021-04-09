using Innovt.Notification.Core.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Notification.Core
{
    public interface INotificationHandler
    {
        Task<dynamic> SendAsync(NotificationMessage message, CancellationToken cancellationToken = default);
    }
}