// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

namespace Innovt.Domain.Adresses;

public class Coordinate
{
    public Coordinate()
    {
    }

    public Coordinate(long latitude, long longitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }

    public int AddressId { get; set; }

    public long Longitude { get; set; }

    public long Latitude { get; set; }
}