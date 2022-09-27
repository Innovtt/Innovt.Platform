// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Dynamo.Tests

using Amazon.DynamoDBv2.DataModel;

namespace Innovt.Cloud.AWS.Dynamo.Tests
{
    [DynamoDBTable("Invoices")]
    public class DataModelTest
    {
        // [DynamoDBHashKey]
        public string BuyerId { get; set; }

        //[DynamoDBRangeKey]
        public string PaymentOrderErpId { get; set; }

        public string PaymentOrderStatus { get; set; }

        public string PaymentOrderStatusId { get; set; }

        public decimal Tax { get; set; }
    }
}