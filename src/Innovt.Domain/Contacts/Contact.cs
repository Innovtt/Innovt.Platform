// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Contacts;
/// <summary>
/// Represents a contact entity.
/// </summary>
public class Contact : ValueObject<int>, IValidatableObject
{
    /// <summary>
    /// Gets or sets the name associated with the contact.
    /// </summary>
    [Required] public string Name { get; set; }

    /// <summary>
    ///     For example Home, Office etc
    /// </summary>
    [Required]
    public string Description { get; set; }
    /// <summary>
    /// Gets or sets the contact type.
    /// </summary>
    [Required] public ContactType Type { get; set; }
    /// <summary>
    /// Gets or sets the contact value (e.g., phone number, email address).
    /// </summary>
    [Required] public string Value { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the contact is deleted.
    /// </summary>
    public bool IsDeleted { get; set; }
    /// <summary>
    /// Validates the contact properties based on the contact type.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A collection of validation results.</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Type == null)
        {
            yield return new ValidationResult("Contact type is required");
        }
        else
        {
            if (!Type.Validate(Value))
                yield return new ValidationResult($"The value {Value} is not valid for {Type.Name}.");
        }
    }
}