// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using Amazon.DynamoDBv2.DataModel;
using Innovt.Cloud.Table;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.DataModel;

/// <summary>
/// Represents a base data model for DynamoDB tables.
/// </summary>
[DynamoDBTable("ServicesAuthorization")]
internal abstract class DataModelBase : ITableMessage
{
    /// <summary>
    /// Gets or sets the sort key for DynamoDB.
    /// </summary>
    [DynamoDBRangeKey("SK")]
    public string Sk { get; set; }

    /// <summary>
    /// Gets or sets the entity type for the data model.
    /// </summary>
    [DynamoDBProperty]
    public string EntityType { get; set; }

    /// <summary>
    /// Gets or sets the hash key for DynamoDB.
    /// </summary>
    [DynamoDBHashKey("PK")]
    public string Id { get; set; }
}