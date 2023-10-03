// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

namespace Innovt.Notification.Core.Domain;
/// <summary>
/// Represents the content of a notification message.
/// </summary>
public class NotificationMessageContent
{
    /// <summary>
    /// Gets or sets the character set of the content.
    /// </summary>
    public string Charset { get; set; }
    /// <summary>
    /// Gets or sets the content.
    /// </summary>
    public string Content { get; set; }
}