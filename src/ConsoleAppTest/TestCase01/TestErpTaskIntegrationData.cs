using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.Table;

namespace ConsoleAppTest.TestCase01;

[DynamoDBTable("ErpTaskIntegrationData")]
public class TestErpTaskIntegrationData : ITableMessage
{
    [DynamoDBHashKey("PK")] public string Id { get; set; }
    [DynamoDBRangeKey("SK")] public string Sk { get; set; }
    public TestTaskIntegration TaskIntegration { get; set; }
    public int? TaskStatus { get; set; }
    public string TaskStatusName { get; set; }
    public string TaskType { get; set; }
    public string SupplierErpId { get; set; }
    public string AnticipationId { get; set; }
    public string Buyer { get; set; }
    public string BuyerGroup { get; set; }
}