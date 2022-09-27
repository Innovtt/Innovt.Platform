// Innovt Company
// Author: Michel Borges
// Project: ConsoleAppTest

using System;
using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.Table;

namespace ConsoleAppTest.DataModels.CapitalSource;

[DynamoDBTable("CapitalSource")]
public class CapitalSourceBaseDataModel : ITableMessage
{
    public CapitalSourceBaseDataModel()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    [DynamoDBHashKey("PK")] public string Pk { get; set; }

    [DynamoDBRangeKey("SK")] public string Sk { get; set; }

    [DynamoDBProperty] public string EntityType { get; protected set; }

    public string EntityTypeCreatedAt
    {
        get => $"ET#{EntityType}#CTA#{CreatedAt:yyyy-MM-ddTHH:mm:ss.000Z}";
        set => _ = value;
    }

    public string EntityTypeUpdatedAt
    {
        get => $"ET#{EntityType}#UTA#{UpdatedAt:yyyy-MM-ddTHH:mm:ss.000Z}";
        set => _ = value;
    }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public string Id { get; set; }
}