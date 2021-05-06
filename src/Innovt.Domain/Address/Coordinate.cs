// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

namespace Innovt.Domain.Address
{
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
}