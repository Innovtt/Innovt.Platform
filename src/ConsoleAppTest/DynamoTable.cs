using Innovt.Cloud.Table;

namespace ConsoleAppTest
{
    //[Amazon.DynamoDBv2.DataModel.DynamoDBTable("UserRoles")]
    //[Amazon.DynamoDBv2.DataModel.DynamoDBTable("BuyerConfiguratorIntegrator")]
    [Amazon.DynamoDBv2.DataModel.DynamoDBTable("Invoices")]
    public class DynamoTable
    {
        [Amazon.DynamoDBv2.DataModel.DynamoDBHashKey()]
        public string BuyerId { get; set; }

        [Amazon.DynamoDBv2.DataModel.DynamoDBRangeKey()]
        public string InvoiceNumber { get; set; }

        public string BuyerDocument { get; set; }
        //   public string Subject { get; set; }
    }
}
