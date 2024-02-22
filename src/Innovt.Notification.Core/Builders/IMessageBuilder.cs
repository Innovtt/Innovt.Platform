// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

using Innovt.Notification.Core.Domain;

namespace Innovt.Notification.Core.Builders;

/// <summary>
///     Interface for message builders.
/// </summary>
public interface IMessageBuilder
{
    /// <summary>
    ///     Builds a notification message based on the provided notification template and request.
    /// </summary>
    /// <param name="notificationTemplate">The notification template.</param>
    /// <param name="request">The notification request.</param>
    /// <returns>The built notification message.</returns>
    NotificationMessage Build(NotificationTemplate notificationTemplate, NotificationRequest request);
}