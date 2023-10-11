// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Adresses;

/// <summary>
/// Represents a city entity.
/// </summary>
public class City : ValueObject
{
    /// <summary>
    /// Gets or sets the name of the city.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the ID of the associated state.
    /// </summary>
    public int StateId { get; set; }

    /// <summary>
    /// Gets or sets the associated state.
    /// </summary>
    public virtual State State { get; set; }
}