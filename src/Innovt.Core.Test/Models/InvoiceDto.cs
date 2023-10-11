// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core.Test

using System;

namespace Innovt.Core.Test.Models;

/// <summary>
/// Represents a Data Transfer Object (DTO) for an invoice with additional anticipation-related properties.
/// </summary>
public class InvoiceDto : Invoice
{
    /// <summary>
    /// Gets or sets the unique identifier of the anticipation request associated with the invoice.
    /// </summary>
    public Guid AnticipationRequestId { get; set; }

    /// <summary>
    /// Gets or sets the date when the anticipation was processed for the invoice. This property can be null if no anticipation has occurred.
    /// </summary>
    public DateTime? AnticipatedAt { get; set; }

    /// <summary>
    /// Gets or sets the early due date for payment of the invoice after anticipation.
    /// </summary>
    public DateTime? EarlyDueDate { get; set; }

    /// <summary>
    /// Gets or sets the net value of the invoice after anticipation.
    /// </summary>
    public decimal EarlyNetValue { get; set; }

    /// <summary>
    /// Gets or sets the rate applied to calculate the anticipation discount.
    /// </summary>
    public decimal AnticipationRate { get; set; }

    /// <summary>
    /// Gets or sets the discount amount applied to the invoice due to anticipation.
    /// </summary>
    public decimal AnticipationDiscount { get; set; }
}