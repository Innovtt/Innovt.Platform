// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

using Innovt.Notification.Core.Domain;

namespace Innovt.Notification.Core;

/// <summary>
///     Interface for a notification handler factory.
/// </summary>
public interface INotificationHandleFactory
{
    /// <summary>
    ///     Creates a notification handler based on the specified notification message type.
    /// </summary>
    /// <param name="notificationMessageType">The notification message type.</param>
    /// <returns>An instance of the notification handler.</returns>
    INotificationHandler Create(NotificationMessageType notificationMessageType);
}