using Amazon.DynamoDBv2.DataModel;

namespace Innovt.Cloud.AWS.Dynamo.Tests.DataModels;

[DynamoDBTable("CloudExperts")]
public class SampleDataModel
{
    [DynamoDBHashKey("PK")] 
    public string Pk { get; set; }

    [DynamoDBRangeKey("SK")] 
    public string Sk { get; set; }
}