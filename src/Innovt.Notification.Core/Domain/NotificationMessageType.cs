// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

namespace Innovt.Notification.Core.Domain;

/// <summary>
///     Enum representing the type of a notification message.
/// </summary>
public enum NotificationMessageType
{
    /// <summary>
    ///     Email notification type.
    /// </summary>
    Email = 0,

    /// <summary>
    ///     SMS notification type.
    /// </summary>
    Sms = 1,

    /// <summary>
    ///     Push notification type.
    /// </summary>
    Push = 2,

    /// <summary>
    ///     Custom notification type.
    /// </summary>
    Custom = 3
}