// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

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