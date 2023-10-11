// Innovt Company
// Author: Michel Borges
// Project: Innovt.Notification.Core

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Collections;

namespace Innovt.Notification.Core.Domain;

/// <summary>
/// Represents a notification request.
/// </summary>
public class NotificationRequest : IValidatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationRequest"/> class.
    /// </summary>
    public NotificationRequest()
    {
        To = new List<NotificationMessageContact>();
    }

    /// <summary>
    /// Gets or sets the template ID.
    /// </summary>
    public string TemplateId { get; set; }

    /// <summary>
    /// Gets or sets the list of "To" contacts for the notification request.
    /// </summary>
    public List<NotificationMessageContact> To { get; set; }

    /// <summary>
    /// Gets or sets the payload associated with the notification request.
    /// </summary>
    public object PayLoad { get; set; }

    /// <inheritdoc />
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (TemplateId.IsNullOrEmpty()) yield return new ValidationResult("TemplateId can't be null or empty.");


        if (To.IsNullOrEmpty())
            yield return new ValidationResult("The To can't be empty.");
        else
            foreach (var to in To)
            {
                var items = to.Validate(validationContext);

                foreach (var item in items)
                    yield return item;
            }
    }
}