using System;
using System.Collections.Generic;

namespace ConsoleAppTest.TestCase01;

public class TestAnticipatePaymentOrdersParametersIntegration
{
    public string Id { get; set; }
    public string AntecipaId { get; set; }
    public string SupplierErpId { get; set; }
    public string BuyerDocument { get; set; }
    public string SupplierDocument { get; set; }
    public string FiscalDocumentNumber { get; set; }
    public DateTime OriginalDueDate { get; set; }
    public string BarCode { get; set; }
    public DateTime EarlyDueDate { get; set; }
    public decimal OriginalNetValue { get; set; }
    public decimal OperationCostPerPaymentOrderAmount { get; set; }
    public decimal OperationCostForEntireTransactionAmount { get; set; }
    public decimal DiscountRate { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal EarlyNetValue { get; set; }
    public IDictionary<string, object> Metadata { get; set; }
}