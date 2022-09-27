// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core.Test

using System;
using Innovt.Domain.Core.Model;

namespace Innovt.Core.Test.Models;

public class Invoice : Entity<Guid>
{
    public Guid BidId { get; set; }
    public string ErpId { get; set; }
    public Guid BuyerId { get; set; }
    public string BuyerName { get; set; }
    public string BuyerErpId { get; set; }
    public string BuyerDocument { get; set; }
    public Guid BuyerGroupId { get; set; }
    public string BuyerGroupName { get; set; }
    public Guid SupplierId { get; set; }
    public string SupplierName { get; set; }
    public string SupplierErpId { get; set; }
    public string SupplierDocument { get; set; }
    public int CurrencyId { get; set; }
    public string Currency { get; set; }
    public string Description { get; set; }
    public decimal Interest { get; set; }
    public decimal Fine { get; set; }
    public decimal Tax { get; set; }
    public string FiscalDocumentNumbers { get; set; }
    public string InstallmentNumber { get; set; }
    public int PaymentTypeId { get; set; }
    public string PaymentType { get; set; }
    public int PaymentOrderStatusId { get; set; }
    public string PaymentOrderStatus { get; set; }
    public string BankSlipBarCode { get; set; }
    public string Metadata { get; set; }
    public int DaysToDue { get; set; }

    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }

    /// <summary>
    ///     Original values  before anticipation
    /// </summary>
    public decimal Discount { get; set; }

    public decimal Value { get; set; }

    public decimal NetValue { get; set; }

    /// <summary>
    ///     Payment infos
    /// </summary>
    public DateTime? PaymentDate { get; set; }

    public decimal? PaymentValue { get; set; }

    public DateTime? UpdatedAt { get; set; }
}