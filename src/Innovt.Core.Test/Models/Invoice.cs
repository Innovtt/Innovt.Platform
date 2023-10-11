// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core.Test

using System;
using Innovt.Domain.Core.Model;

namespace Innovt.Core.Test.Models;

/// <summary>
/// Represents an invoice entity with various properties related to billing and payment information.
/// </summary>
public class Invoice : Entity<Guid>
{
    /// <summary>
    /// Gets or sets the unique identifier of the associated business entity.
    /// </summary>
    public Guid BidId { get; set; }

    /// <summary>
    /// Gets or sets the ERP (Enterprise Resource Planning) identifier associated with the invoice.
    /// </summary>
    public string ErpId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the buyer.
    /// </summary>
    public Guid BuyerId { get; set; }

    /// <summary>
    /// Gets or sets the name of the buyer.
    /// </summary>
    public string BuyerName { get; set; }

    /// <summary>
    /// Gets or sets the ERP identifier associated with the buyer.
    /// </summary>
    public string BuyerErpId { get; set; }

    /// <summary>
    /// Gets or sets the document information associated with the buyer.
    /// </summary>
    public string BuyerDocument { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the buyer group.
    /// </summary>
    public Guid BuyerGroupId { get; set; }

    /// <summary>
    /// Gets or sets the name of the buyer group.
    /// </summary>
    public string BuyerGroupName { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the supplier.
    /// </summary>
    public Guid SupplierId { get; set; }

    /// <summary>
    /// Gets or sets the name of the supplier.
    /// </summary>
    public string SupplierName { get; set; }

    /// <summary>
    /// Gets or sets the ERP identifier associated with the supplier.
    /// </summary>
    public string SupplierErpId { get; set; }

    /// <summary>
    /// Gets or sets the document information associated with the supplier.
    /// </summary>
    public string SupplierDocument { get; set; }

    /// <summary>
    /// Gets or sets the currency identifier.
    /// </summary>
    public int CurrencyId { get; set; }

    /// <summary>
    /// Gets or sets the currency.
    /// </summary>
    public string Currency { get; set; }

    /// <summary>
    /// Gets or sets the description of the invoice.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the interest amount.
    /// </summary>
    public decimal Interest { get; set; }

    /// <summary>
    /// Gets or sets the fine amount.
    /// </summary>
    public decimal Fine { get; set; }

    /// <summary>
    /// Gets or sets the tax amount.
    /// </summary>
    public decimal Tax { get; set; }

    /// <summary>
    /// Gets or sets the fiscal document numbers associated with the invoice.
    /// </summary>
    public string FiscalDocumentNumbers { get; set; }

    /// <summary>
    /// Gets or sets the installment number.
    /// </summary>
    public string InstallmentNumber { get; set; }

    /// <summary>
    /// Gets or sets the payment type identifier.
    /// </summary>
    public int PaymentTypeId { get; set; }

    /// <summary>
    /// Gets or sets the payment type.
    /// </summary>
    public string PaymentType { get; set; }

    /// <summary>
    /// Gets or sets the payment order status identifier.
    /// </summary>
    public int PaymentOrderStatusId { get; set; }

    /// <summary>
    /// Gets or sets the payment order status.
    /// </summary>
    public string PaymentOrderStatus { get; set; }

    /// <summary>
    /// Gets or sets the bank slip barcode.
    /// </summary>
    public string BankSlipBarCode { get; set; }

    /// <summary>
    /// Gets or sets metadata associated with the invoice.
    /// </summary>
    public string Metadata { get; set; }

    /// <summary>
    /// Gets or sets the number of days until the due date of the invoice.
    /// </summary>
    public int DaysToDue { get; set; }

    /// <summary>
    /// Gets or sets the date when the invoice was issued.
    /// </summary>
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// Gets or sets the due date for payment of the invoice.
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    ///     Original values  before anticipation
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets or sets the total value of the invoice, including all charges, taxes, and discounts.
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Gets or sets the net value of the invoice, which represents the total value after applying discounts and taxes.
    /// </summary>
    public decimal NetValue { get; set; }


    /// <summary>
    /// Gets or sets the date when the payment for the invoice was made. This property can be null if the payment has not been made yet.
    /// </summary>
    public DateTime? PaymentDate { get; set; }

    /// <summary>
    /// Gets or sets the amount paid for the invoice. This property can be null if the payment has not been made yet.
    /// </summary>
    public decimal? PaymentValue { get; set; }

    /// <summary>
    /// Gets or sets the date when the invoice was last updated in the system. This property can be null if the invoice has not been updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}