// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Adresses;

public class Country : ValueObject
{
    public static Country Brazil = new() { Id = 1, Name = "Brasil", Code = 55, ISOCode = "BRA" };

    public string Name { get; set; }

    public int Code { get; set; }

    public string ISOCode { get; set; }

    public string Nationality { get; set; }
}