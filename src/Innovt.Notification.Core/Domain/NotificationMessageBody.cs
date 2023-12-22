// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

namespace Innovt.Notification.Core.Domain;

/// <summary>
///     Represents the body of a notification message.
/// </summary>
public class NotificationMessageBody : NotificationMessageContent
{
    /// <summary>
    ///     Gets or sets a value indicating whether the content is in HTML format.
    /// </summary>
    public bool IsHtml { get; set; }
}