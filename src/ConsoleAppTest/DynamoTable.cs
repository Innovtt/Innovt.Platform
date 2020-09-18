using Innovt.Cloud.Table;

namespace ConsoleAppTest
{
    [Amazon.DynamoDBv2.DataModel.DynamoDBTable("NotificationTemplate")]
    public class DynamoTable:ITableMessage
    {
        [Amazon.DynamoDBv2.DataModel.DynamoDBHashKey()]
        public string Id { get; set; }

        //[Amazon.DynamoDBv2.DataModel.DynamoDBRangeKey("PartitionKey")]
        public string RangeKey { get; set; }
        public string Subject { get; set; }
    }
}
