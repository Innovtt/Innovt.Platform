using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Integration;

[DynamoDBTable("ChangeTracking")]
internal sealed class TrackingTestEntity
{
    [DynamoDBHashKey("PK")]
    public string Pk { get; set; } = string.Empty;

    [DynamoDBRangeKey("SK")]
    public string Sk { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public int Counter { get; set; }
    public TrackingAddress? Home { get; set; }
    public List<string>? Tags { get; set; }
}

internal sealed class TrackingAddress
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int ZipCode { get; set; }
}
