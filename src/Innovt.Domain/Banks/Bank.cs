// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-06-22
// Contact: michel@innovt.com.br or michelmob@gmail.com

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