// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System;

namespace Innovt.Cloud.Table;

/// <summary>
/// Represents a request for querying a data source.
/// </summary>
public class QueryRequest : BaseRequest, ICloneable
{
    /// <summary>
    /// Gets or sets the key condition expression used in the query.
    /// </summary>
    public string KeyConditionExpression { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to scan the index forward or backward.
    /// </summary>
    public bool ScanIndexForward { get; set; }

    /// <summary>
    /// Creates a deep copy of the current instance.
    /// </summary>
    /// <returns>A new instance of <see cref="QueryRequest"/> that is a copy of the current instance.</returns>
    public object Clone()
    {
        return new QueryRequest
        {
            AttributesToGet = AttributesToGet,
            Filter = Filter,
            KeyConditionExpression = KeyConditionExpression,
            FilterExpression = FilterExpression,
            IndexName = IndexName,
            ScanIndexForward = ScanIndexForward,
            PageSize = PageSize,
            Page = Page
        };
    }
}