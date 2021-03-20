using Innovt.Notification.Core.Domain;

namespace Innovt.Notification.Core.Builders
{
    public interface IMessageBuilder
    {
        NotificationMessage Build(NotificationTemplate template, NotificationRequest request);
    }
}