// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Adresses;

public class City : ValueObject
{
    public string Name { get; set; }

    public int StateId { get; set; }

    public virtual State State { get; set; }
}