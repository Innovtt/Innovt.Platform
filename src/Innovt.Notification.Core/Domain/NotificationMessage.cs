// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Innovt.Core.Collections;
using Innovt.Core.Utilities;

namespace Innovt.Notification.Core.Domain;

/// <summary>
/// Represents a notification message.
/// </summary>
public class NotificationMessage : IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationMessage"/> class with the specified type.
    /// </summary>
    /// <param name="type">The type of the notification message.</param>
    public NotificationMessage(NotificationMessageType type)
    {
        Type = type;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationMessage"/> class with the specified type, from address, from name, and subject.
    /// </summary>
    /// <param name="type">The type of the notification message.</param>
    /// <param name="fromAddress">The "From" address.</param>
    /// <param name="fromName">The "From" name.</param>
    /// <param name="subject">The subject of the notification.</param>
    public NotificationMessage(NotificationMessageType type, string fromAddress, string fromName, string subject)
    {
        Type = type;

        if (fromAddress.IsNotNullOrEmpty()) AddFrom(fromAddress, fromName);

        if (subject.IsNotNullOrEmpty()) AddSubject(subject);
    }

    /// <summary>
    /// Gets or sets the "From" contact information for the notification message.
    /// </summary>
    public NotificationMessageContact From { get; internal set; }

    /// <summary>
    /// Gets or sets the list of "To" contact information for the notification message.
    /// </summary>
    public IList<NotificationMessageContact> To { get; internal set; }

    /// <summary>
    /// Gets or sets the list of "Bcc" contact information for the notification message.
    /// </summary>
    public IList<NotificationMessageContact> BccTo { get; internal set; }

    /// <summary>
    /// Gets or sets the list of "Cc" contact information for the notification message.
    /// </summary>
    public IList<NotificationMessageContact> CcTo { get; internal set; }

    /// <summary>
    /// Gets or sets the list of "Reply-To" contact information for the notification message.
    /// </summary>
    public IList<NotificationMessageContact> ReplyToAddresses { get; internal set; }

    /// <summary>
    /// Gets or sets the type of the notification message.
    /// </summary>
    public NotificationMessageType Type { get; set; }

    /// <summary>
    /// Gets or sets the subject of the notification message.
    /// </summary>
    public NotificationMessageSubject Subject { get; set; }

    /// <summary>
    /// Gets or sets the body of the notification message.
    /// </summary>
    public NotificationMessageBody Body { get; set; }

    /// <inheritdoc />
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (To == null || !To.Any()) yield return new ValidationResult("Invalid value for To", new[] { "To" });

        if (Body == null || Body.Content.IsNullOrEmpty())
            yield return new ValidationResult("Invalid value for Body", new[] { "Body" });

        if (From == null || From.Address.IsNullOrEmpty())
            yield return new ValidationResult("Invalid value for From", new[] { "From" });

        if (Type != NotificationMessageType.Sms || To == null) yield break;


        foreach (var to in To)
            if (to.Address == null ||
                !to.Address.StartsWith("+", StringComparison.InvariantCultureIgnoreCase))
                yield return new ValidationResult(
                    "Invalid value for To that should start with + and E.164 format.", new[] { "To" });
    }

    /// <summary>
    /// Adds the subject to the notification message.
    /// </summary>
    public NotificationMessage AddSubject(string subject, string charset = null)
    {
        Subject = new NotificationMessageSubject
        {
            Charset = charset.GetValueOrDefault("UTF-8"),
            Content = subject
        };

        return this;
    }

    /// <summary>
    /// Adds the "From" contact information to the notification message.
    /// </summary>
    public NotificationMessage AddFrom(string address, string name = null)
    {
        From = new NotificationMessageContact(name, address);

        return this;
    }

    /// <summary>
    /// Adds "To" contact information to the notification message.
    /// </summary>
    public NotificationMessage AddTo(string address, string name = null)
    {
        To = To.AddFluent(new NotificationMessageContact(name, address));

        return this;
    }

    /// <summary>
    /// Adds "Bcc" contact information to the notification message.
    /// </summary>
    public virtual NotificationMessage AddBccTo(string address, string name = null)
    {
        BccTo = BccTo.AddFluent(new NotificationMessageContact(name, address));

        return this;
    }

    /// <summary>
    /// Adds "Cc" contact information to the notification message.
    /// </summary>
    public virtual NotificationMessage AddCcTo(string address, string name = null)
    {
        CcTo = CcTo.AddFluent(new NotificationMessageContact(name, address));

        return this;
    }

    /// <summary>
    /// Adds "Reply-To" contact information to the notification message.
    /// </summary>
    public virtual NotificationMessage AddReplyTo(string address, string name = null)
    {
        ReplyToAddresses = ReplyToAddresses.AddFluent(new NotificationMessageContact(name, address));

        return this;
    }
}