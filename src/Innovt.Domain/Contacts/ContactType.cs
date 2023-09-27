// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Innovt.Core.Utilities;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Contacts;

/// <summary>
///     Email, Phone
/// </summary>
/// /// <summary>
/// Represents a contact type entity.
/// </summary>
public class ContactType : ValueObject<int>
{
    private ContactType()
    {
    }
    /// <summary>
    /// Gets or sets the name of the contact type.
    /// </summary>
    [Required] public string Name { get; set; }
    /// <summary>
    /// Gets or sets the description of the contact type.
    /// </summary>
    [Required] public string Description { get; set; }
    /// <summary>
    /// Gets or sets the URL to an icon associated with the contact type.
    /// </summary>
    public string IconUrl { get; set; }
    /// <summary>
    /// Gets or sets the regular expression for validation of contact values.
    /// </summary>
    public string RegexValidation { get; set; }
    /// <summary>
    /// Creates a new contact type with the given name and description.
    /// </summary>
    /// <param name="name">The name of the contact type.</param>
    /// <param name="description">The description of the contact type.</param>
    /// <returns>A new <see cref="ContactType"/> instance.</returns>
    public static ContactType Create(string name, string description)
    {
        return new ContactType
        {
            Name = name,
            Description = description
        };
    }
    /// <summary>
    /// Validates a contact value based on the regex validation.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns>True if the value is valid according to the regex; otherwise, false.</returns>
    public virtual bool Validate(string value)
    {
        if (RegexValidation.IsNullOrEmpty())
            return true;

        var regEx = new Regex(RegexValidation);

        return regEx.IsMatch(value);
    }
}