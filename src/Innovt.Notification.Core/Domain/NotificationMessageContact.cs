// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Collections;

namespace Innovt.Notification.Core.Domain;

/// <summary>
/// Represents a contact for a notification message.
/// </summary>
public class NotificationMessageContact : IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationMessageContact"/> class with the specified name and address.
    /// </summary>
    /// <param name="name">The name of the contact.</param>
    /// <param name="address">The address of the contact.</param>
    public NotificationMessageContact(string name, string address)
    {
        Name = name;
        Address = address;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationMessageContact"/> class.
    /// </summary>
    public NotificationMessageContact()
    {
    }

    /// <summary>
    /// Gets or sets the name of the contact.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the address of the contact.
    /// </summary>
    public string Address { get; set; }

    /// <inheritdoc />
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Address.IsNullOrEmpty()) yield return new ValidationResult("Invalid address");
    }
}