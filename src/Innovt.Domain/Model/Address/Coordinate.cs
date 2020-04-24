namespace Innovt.Domain.Model.Address
{
    public class Coordinate
    {
        public int AddressId { get; set; }

        public long Longitude { get; set; }

        public long Latitude { get; set; }

        public Coordinate() { }

        public Coordinate(long latitude, long longitude)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
        }
    }
}
