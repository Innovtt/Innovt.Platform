// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Address
{
    public class Address : ValueObject
    {
        public AddressType Type { get; set; }
        public int TypeId { get; set; }

        public string Description { get; set; }

        public string Street { get; set; }

        public string Complement { get; set; }

        public string Neighborhood { get; set; }

        public string Number { get; set; }

        public string ZipCode { get; set; }

        public City City { get; set; }

        public int CityId { get; set; }

        public Coordinate Coordinate { get; set; }

        public bool IsDeleted { get; set; }
    }
}