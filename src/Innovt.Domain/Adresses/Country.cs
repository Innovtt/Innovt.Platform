// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Adresses;

/// <summary>
///     Represents a country entity.
/// </summary>
public class Country : ValueObject
{
    /// <summary>
    ///     Predefined country: Brazil.
    /// </summary>
    public static Country Brazil = new() { Id = 1, Name = "Brasil", Code = 55, ISOCode = "BRA" };

    /// <summary>
    ///     Gets or sets the name of the country.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets the country calling code.
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    ///     Gets or sets the ISO country code.
    /// </summary>
    public string ISOCode { get; set; }

    /// <summary>
    ///     Gets or sets the nationality associated with the country.
    /// </summary>
    public string Nationality { get; set; }
}