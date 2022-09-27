// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.Table;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.DataModel;

[DynamoDBTable("ServicesAuthorization")]
internal abstract class DataModelBase : ITableMessage
{
    [DynamoDBRangeKey("SK")] public string Sk { get; set; }

    [DynamoDBProperty] public string EntityType { get; set; }

    [DynamoDBHashKey("PK")] public string Id { get; set; }
}