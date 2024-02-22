// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Banks;

/// <summary>
///     Represents a bank entity.
/// </summary>
public class Bank : ValueObject
{
    /// <summary>
    ///     Gets or sets the name of the bank.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets the code of the bank.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    ///     Gets or sets the routing number of the bank.
    /// </summary
    public string RoutingNumber { get; set; }

    /// <summary>
    ///     Gets or sets the account number associated with the bank.
    /// </summary>
    public string AccountNumber { get; set; }

    /// <summary>
    ///     Gets or sets the account digit associated with the account number.
    /// </summary>
    public string AccountDigit { get; set; }

    /// <summary>
    ///     Gets or sets the account type (e.g., Checking, Savings).
    /// </summary>
    public AccountType AccountType { get; set; }
}