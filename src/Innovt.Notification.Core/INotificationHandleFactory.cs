// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Notification.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Notification.Core.Domain;

namespace Innovt.Notification.Core;

public interface INotificationHandleFactory
{
    INotificationHandler Create(NotificationMessageType notificationMessageType);
}