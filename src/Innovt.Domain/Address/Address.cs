// Company: INNOVT
// Project: Innovt.Domain
// Created By: Michel Borges
// Date; 18

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