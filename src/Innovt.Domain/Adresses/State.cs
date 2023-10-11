// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using System.Collections.Generic;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Adresses;

/// <summary>
/// Represents a state entity.
/// </summary>
public class State : ValueObject
{
    /// <summary>
    /// Gets or sets the description or name of the state.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the acronym for the state.
    /// </summary>
    public string Acronym { get; set; }

    /// <summary>
    /// Gets or sets the UTC offset for the state.
    /// </summary>
    public string UtcOffset { get; set; }

    /// <summary>
    /// Gets or sets the country ID associated with the state.
    /// </summary>
    public int CountryId { get; set; }

    /// <summary>
    /// Gets or sets the associated country.
    /// </summary>
    public virtual Country Country { get; set; }

    /// <summary>
    /// Gets or sets the list of cities within the state.
    /// </summary>
    public virtual IList<City> Cities { get; set; }
}