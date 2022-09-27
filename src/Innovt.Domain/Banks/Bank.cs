// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Banks;

public class Bank : ValueObject
{
    public string Name { get; set; }

    public string Code { get; set; }

    public string RoutingNumber { get; set; }

    public string AccountNumber { get; set; }

    public string AccountDigit { get; set; }

    public AccountType AccountType { get; set; }
}