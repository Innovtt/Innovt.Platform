// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

using Innovt.Notification.Core.Domain;

namespace Innovt.Notification.Core.Builders;

public interface IMessageBuilder
{
    NotificationMessage Build(NotificationTemplate notificationTemplate, NotificationRequest request);
}