// Innovt Company
// Author: Michel Borges
// Project: ConsoleAppTest

using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.Table;

namespace ConsoleAppTest.DataModels;

[DynamoDBTable("InvoiceKpiProgress")]
public class KpiBaseDataModel : ITableMessage
{
    [DynamoDBHashKey] public string PK { get; set; }

    [DynamoDBRangeKey] public string SK1 { get; set; }

    public string Id { get; set; }
}

public class KpiProgressDataModel : KpiBaseDataModel
{
    public string SK2 { get; set; }
    //public string KpiType { get; set; }

    //public bool Success { get; set; }

    //public string Message { get; set; }

    //public DateTime CreatedAt { get; set; }

    //public string SupplierId { get; set; }

    //public string BuyerId { get; set; }

    //public int ApproximateReceiveCount { get; set; }

    public PaymentOrder NewPaymentOrder { get; set; }

    public PaymentOrder OldPaymentOrder { get; set; }

    //public KpiProgressDataModel()
    //{
    //    CreatedAt = DateTime.UtcNow;
    //}

    //public void SetFailed(string reason)
    //{
    //    Success = false;
    //    Message = reason;
    //    ApproximateReceiveCount++;
    //}
}