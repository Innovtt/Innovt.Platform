// Innovt Company
// Author: Michel Borges
// Project: ConsoleAppTest

using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.Table;

namespace ConsoleAppTest.DataModels.Anticipation;

[DynamoDBTable("Anticipation")]
public abstract class BaseDataModel : ITableMessage
{
    [DynamoDBHashKey("PK")] public string Pk { get; set; }

    [DynamoDBRangeKey("SK")] public string Sk { get; set; }

    [DynamoDBProperty] protected string EntityType { get; set; }
    public string Id { get; set; }
}