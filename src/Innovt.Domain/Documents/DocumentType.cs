// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using Innovt.Domain.Adresses;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Documents;

public class DocumentType : ValueObject
{
    public string Name { get; set; }

    public int CountryId { get; set; }

    public Country Country { get; set; }

    public string Mask { get; set; }
}