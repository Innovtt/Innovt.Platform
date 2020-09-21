using Innovt.Cloud.Table;

namespace ConsoleAppTest
{
    //[Amazon.DynamoDBv2.DataModel.DynamoDBTable("UserRoles")]
    [Amazon.DynamoDBv2.DataModel.DynamoDBTable("BuyerGroupConfiguratorIntegrator")]
    public class DynamoTable:ITableMessage
    {
        [Amazon.DynamoDBv2.DataModel.DynamoDBHashKey()]
        public string Id { get; set; }

      // [Amazon.DynamoDBv2.DataModel.DynamoDBRangeKey("RangeKey")]
       //public string RangeKey { get; set; }
     //   public string Subject { get; set; }
    }
}
