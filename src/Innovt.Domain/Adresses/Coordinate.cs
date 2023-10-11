// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

namespace Innovt.Domain.Adresses;

/// <summary>
/// Represents a geographical coordinate.
/// </summary>
public class Coordinate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Coordinate"/> class.
    /// </summary>
    public Coordinate()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Coordinate"/> class with latitude and longitude.
    /// </summary>
    /// <param name="latitude">The latitude value.</param>
    /// <param name="longitude">The longitude value.</param>
    public Coordinate(long latitude, long longitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }

    /// <summary>
    /// Gets or sets the address ID associated with the coordinate.
    /// </summary>
    public int AddressId { get; set; }

    /// <summary>
    /// Gets or sets the longitude value of the coordinate.
    /// </summary>
    public long Longitude { get; set; }

    /// <summary>
    /// Gets or sets the latitude value of the coordinate.
    /// </summary>
    public long Latitude { get; set; }
}