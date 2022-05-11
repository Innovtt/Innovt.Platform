// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Domain.Core.Model;
using System.Collections.Generic;

namespace Innovt.Domain.Adresses;

public class State : ValueObject
{
    public string Description { get; set; }

    public string Acronym { get; set; }

    public string UtcOffset { get; set; }

    public int CountryId { get; set; }

    public virtual Country Country { get; set; }

    public virtual IList<City> Cities { get; set; }
}