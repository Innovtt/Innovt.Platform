// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using System;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain;

/// <summary>
/// Represents a base credit card information.
/// </summary>
public class BaseCreditCard : ValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseCreditCard"/> class with credit card details.
    /// </summary>
    /// <param name="number">The credit card number.</param>
    /// <param name="holder">The cardholder's name.</param>
    /// <param name="expiration">The expiration date of the credit card.</param>
    /// <param name="securityCode">The security code of the credit card.</param>
    public BaseCreditCard(string number, string holder, DateTime expiration, string securityCode)
    {
        Number = number;
        Holder = holder;
        Expiration = expiration;
        SecurityCode = securityCode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseCreditCard"/> class with a token and security code.
    /// </summary>
    /// <param name="token">The token associated with the credit card.</param>
    /// <param name="securityCode">The security code of the credit card.</param>
    public BaseCreditCard(string token, string securityCode)
    {
        SecurityCode = securityCode;
        Token = token;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseCreditCard"/> class.
    /// </summary>
    public BaseCreditCard()
    {
    }

    /// <summary>
    /// Gets or sets the credit card number.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Gets or sets the cardholder's name.
    /// </summary>
    public string Holder { get; set; }

    /// <summary>
    /// Gets or sets the security code of the credit card.
    /// </summary>
    public string SecurityCode { get; set; }

    /// <summary>
    /// Gets or sets the token associated with the credit card.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets the expiration date of the credit card.
    /// </summary>
    public DateTime Expiration { get; set; }

    /// <summary>
    /// Gets the masked credit card number with only the last four digits visible.
    /// </summary>
    public string MaskedNumber
    {
        get
        {
            var maskedNumber = GetMaskedCreditCard(Number);

            return maskedNumber;
        }
    }

    /// <summary>
    /// Generates a masked credit card number with only the first six and last four digits visible.
    /// </summary>
    /// <param name="number">The original credit card number.</param>
    /// <returns>The masked credit card number.</returns>
    public static string GetMaskedCreditCard(string number)
    {
        var result = "******";

        if (number != null && number.Length > 10)
        {
            var first = number.Substring(0, 6);
            var last = number.Substring(number.Length - 4, 4);

            result = first + result + last;
        }

        return result;
    }
}