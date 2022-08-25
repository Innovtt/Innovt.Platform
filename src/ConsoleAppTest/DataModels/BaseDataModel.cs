using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.Table;
using System;

namespace ConsoleAppTest.DataModels;

[DynamoDBTable("CapitalSource")]
public class BaseDataModel : ITableMessage
{
    [DynamoDBHashKey("PK")]
    public string Pk { get; set; }
    [DynamoDBRangeKey("SK")]
    public string Sk { get; set; }
    [DynamoDBProperty]
    public string EntityType { get; protected set; }
    public string Id { get; set; }
    public string EntityTypeCreatedAt { get => $"ET#{EntityType}#CTA#{CreatedAt:yyyy-MM-ddTHH:mm:ss.000Z}"; set => _ = value; }
    public string EntityTypeUpdatedAt { get => $"ET#{EntityType}#UTA#{UpdatedAt:yyyy-MM-ddTHH:mm:ss.000Z}"; set => _ = value; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public BaseDataModel()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}