// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Adresses;

/// <summary>
/// Represents an address entity.
/// </summary>
public class Address : ValueObject
{
    /// <summary>
    /// Gets or sets the type of address.
    /// </summary>
    public AddressType Type { get; set; }

    /// <summary>
    /// Gets or sets the ID of the address type.
    /// </summary>
    public int TypeId { get; set; }

    /// <summary>
    /// Gets or sets the description of the address.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the street of the address.
    /// </summary>
    public string Street { get; set; }

    /// <summary>
    /// Gets or sets additional details or complement for the address.
    /// </summary>
    public string Complement { get; set; }

    /// <summary>
    /// Gets or sets the neighborhood or area of the address.
    /// </summary>
    public string Neighborhood { get; set; }

    /// <summary>
    /// Gets or sets the number associated with the address.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Gets or sets the ZIP code of the address.
    /// </summary>
    public string ZipCode { get; set; }

    /// <summary>
    /// Gets or sets the city associated with the address.
    /// </summary>
    public City City { get; set; }

    /// <summary>
    /// Gets or sets the ID of the associated city.
    /// </summary>
    public int CityId { get; set; }

    /// <summary>
    /// Gets or sets the coordinates of the address.
    /// </summary>
    public Coordinate Coordinate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the address is marked as deleted.
    /// </summary>
    public bool IsDeleted { get; set; }
}