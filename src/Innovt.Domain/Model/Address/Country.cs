// Company: INNOVT
// Project: Innovt.Domain
// Created By: Michel Borges
// Date: 2016/10/18

namespace Innovt.Domain.Model.Address
{
    public class Country : ValueObject
    {
        public static Country Brazil = new Country {Id = 1,  Name = "Brasil", Code =55, ISOCode = "BRA"};

        public string Name { get; set; }

        public int Code { get; set; }
        public string ISOCode { get; set; }

        public string Nationality { get; set; }
    }
}