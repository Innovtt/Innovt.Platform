// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Address
{
    public class Country : ValueObject
    {
        public static Country Brazil = new() {Id = 1, Name = "Brasil", Code = 55, ISOCode = "BRA"};

        public string Name { get; set; }

        public int Code { get; set; }

        public string ISOCode { get; set; }

        public string Nationality { get; set; }
    }
}