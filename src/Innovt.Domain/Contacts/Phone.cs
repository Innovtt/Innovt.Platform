// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Contacts;

public class Phone : ValueObject
{
    public string CountryCode { get; set; }
    public string AreaCode { get; set; }
    public string Number { get; set; }
    public string Extension { get; set; }
}