using System;

namespace Innovt.Core.Test.Models;

public class InvoiceDto : Invoice
{
    public Guid AnticipationRequestId { get; set; }

    /// <summary>
    ///     Anticipation info
    /// </summary>
    public DateTime? AnticipatedAt { get; set; }

    public DateTime? EarlyDueDate { get; set; }
    public decimal EarlyNetValue { get; set; }
    public decimal AnticipationRate { get; set; }
    public decimal AnticipationDiscount { get; set; }
}