using Innovt.Notification.Core.Domain;

namespace Innovt.Notification.Core
{
    public interface INotificationHandleFactory
    {
        INotificationHandler Create(NotificationMessageType type);
    }
}