// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

namespace Innovt.Notification.Core.Domain;

public class NotificationTemplate
{
    /// <summary>
    /// Gets or sets the ID of the notification template.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the subject of the notification template.
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Gets or sets the "From" name of the notification template.
    /// </summary>
    public string FromName { get; set; }

    /// <summary>
    /// Gets or sets the "From" address of the notification template.
    /// </summary>
    public string FromAddress { get; set; }

    /// <summary>
    /// Gets or sets the URL of the notification template.
    /// </summary>
    public string TemplateUrl { get; set; }

    /// <summary>
    /// Gets or sets the character set of the notification template.
    /// </summary>
    public string Charset { get; set; }

    /// <summary>
    /// Gets or sets the body of the notification template.
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// Gets or sets the builder associated with the notification template.
    /// </summary>
    public string Builder { get; set; }

    /// <summary>
    /// Gets or sets the type of the notification template.
    /// </summary>
    public NotificationMessageType Type { get; set; }
}