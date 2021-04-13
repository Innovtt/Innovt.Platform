// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Dynamo.Tests
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Amazon.DynamoDBv2.DataModel;

namespace Innovt.Cloud.AWS.Dynamo.Tests
{
    [DynamoDBTable("Invoices")]
    public class DataModel
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