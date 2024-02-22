// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Notification

using System;
using Innovt.Core.CrossCutting.Ioc;
using Innovt.Notification.Core;
using Innovt.Notification.Core.Domain;

namespace Innovt.Cloud.AWS.Notification;

/// <summary>
///     Factory for creating notification handlers based on the notification message type.
/// </summary>
public class NotificationHandleFactory : INotificationHandleFactory
{
    private readonly IContainer container;

    /// <summary>
    ///     Initializes a new instance of the <see cref="NotificationHandleFactory" /> class.
    /// </summary>
    /// <param name="container">The IoC container.</param>
    public NotificationHandleFactory(IContainer container)
    {
        this.container = container ?? throw new ArgumentNullException(nameof(container));
    }

    /// <summary>
    ///     Creates a notification handler based on the provided notification message type.
    /// </summary>
    /// <param name="notificationMessageType">The notification message type.</param>
    /// <returns>A notification handler for the specified message type.</returns>
    public virtual INotificationHandler Create(NotificationMessageType notificationMessageType)
    {
        return notificationMessageType switch
        {
            NotificationMessageType.Email => container.Resolve<MailNotificationHandler>(),
            NotificationMessageType.Sms => container.Resolve<SmsNotificationHandler>(),
            _ => CreateCustomHandle(notificationMessageType)
        };
    }

    /// <summary>
    ///     Creates a custom notification handler for the specified message type.
    ///     Override this method to implement custom handling for specific message types.
    /// </summary>
    /// <param name="notificationMessageType">The notification message type.</param>
    /// <returns>A custom notification handler.</returns>
    public virtual INotificationHandler CreateCustomHandle(NotificationMessageType notificationMessageType)
    {
        return null;
    }
}