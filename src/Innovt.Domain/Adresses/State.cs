// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using System.Collections.Generic;
using Innovt.Domain.Core.Model;

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