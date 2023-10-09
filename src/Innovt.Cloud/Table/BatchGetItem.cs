// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System.Collections.Generic;

namespace Innovt.Cloud.Table;
/// <summary>
/// Represents a request to batch get items from a database.
/// </summary>
public class BatchGetItem
{
    /// <summary>
    /// Gets or sets a value indicating whether a consistent read is requested.
    /// </summary>
    public bool ConsistentRead { get; set; }
    /// <summary>
    /// Gets or sets the expression attribute names used in the request.
    /// </summary>
    public Dictionary<string, string> ExpressionAttributeNames { get; set; }
    /// <summary>
    /// Gets or sets the keys for which items will be retrieved in the batch.
    /// Each key is represented as a dictionary of attribute names and values.
    /// </summary>
    public List<Dictionary<string, object>> Keys { get; set; }
    /// <summary>
    /// Gets or sets the projection expression for retrieving specific attributes.
    /// </summary>
    public string ProjectionExpression { get; set; }
}