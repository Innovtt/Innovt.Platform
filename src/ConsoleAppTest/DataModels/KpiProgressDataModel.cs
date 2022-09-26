using System;
using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.Table;

namespace ConsoleAppTest.DataModels
{
    [DynamoDBTable("InvoiceKpiProgress")]
    public class KpiProgressDataModel : ITableMessage
    {
        public string Id { get; set; }

        [DynamoDBHashKey]
        public string PK { get => !string.IsNullOrWhiteSpace(Id) ? $"E#{Id}" : null; set => _ = value; }

        [DynamoDBRangeKey]
        public string SK1 { get => $"K#{KpiType}"; set => _ = value; }

        public string SK2 { get => $"R#{Success}"; set => _ = value; }

        public string KpiType { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }

        public string SupplierId { get; set; }

        public string BuyerId { get; set; }

        public int ApproximateReceiveCount { get; set; }

        public PaymentOrder NewPaymentOrder { get; set; }

        public PaymentOrder OldPaymentOrder { get; set; }

        public KpiProgressDataModel()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public void SetFailed(string reason)
        {
            Success = false;
            Message = reason;
            ApproximateReceiveCount++;
        }
    }
}