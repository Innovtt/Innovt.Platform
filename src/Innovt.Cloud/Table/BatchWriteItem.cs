// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System.Collections.Generic;

namespace Innovt.Cloud.Table;
/// <summary>
/// Represents a request to batch write items to a database.
/// </summary>
public class BatchWriteItem
{
    /// <summary>
    /// Gets or sets a collection of items to be added or updated in the batch.
    /// The key represents the table name and the value represents the item to put.
    /// </summary>
    public Dictionary<string, object> PutRequest { get; set; }
    /// <summary>
    /// Gets or sets a collection of items to be deleted in the batch.
    /// The key represents the table name and the value represents the item to delete.
    /// </summary>
    public Dictionary<string, object> DeleteRequest { get; set; }
}