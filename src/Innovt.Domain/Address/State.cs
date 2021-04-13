// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Address
{
    public class State : ValueObject
    {
        public string Description { get; set; }

        public string Acronym { get; set; }

        public string UtcOffset { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual IList<City> Cities { get; set; }
    }
}