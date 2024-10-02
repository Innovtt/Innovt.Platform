// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Contacts;

/// <summary>
///     Represents a phone entity.
/// </summary>
public class Phone : ValueObject
{
    /// <summary>
    ///     Gets or sets the country code associated with the phone number.
    /// </summary>
    public string CountryCode { get; set; }

    /// <summary>
    ///     Gets or sets the area code associated with the phone number.
    /// </summary>
    public string AreaCode { get; set; }

    /// <summary>
    ///     Gets or sets the main phone number.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    ///     Gets or sets the phone extension, if applicable.
    /// </summary>
    public string Extension { get; set; }
}