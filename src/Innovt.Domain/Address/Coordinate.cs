namespace Innovt.Domain.Address
{
    public class Coordinate
    {
        public int AddressId { get; set; }

        public long Longitude { get; set; }

        public long Latitude { get; set; }

        public Coordinate()
        {
        }

        public Coordinate(long latitude, long longitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}