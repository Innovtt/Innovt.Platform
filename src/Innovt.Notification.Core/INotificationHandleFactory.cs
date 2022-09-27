// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

using Innovt.Notification.Core.Domain;

namespace Innovt.Notification.Core;

public interface INotificationHandleFactory
{
    INotificationHandler Create(NotificationMessageType notificationMessageType);
}