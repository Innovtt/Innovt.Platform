using Amazon.DynamoDBv2.DataModel;

namespace Innovt.Cloud.AWS.Dynamo.Tests.DataModels;

[DynamoDBTable("CloudExperts")]
public class SampleDataModel
{
    [DynamoDBHashKey("PK")] 
    public string PK { get; set; }

    [DynamoDBRangeKey("SK")] 
    public string SK { get; set; }
}