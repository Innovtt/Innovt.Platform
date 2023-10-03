// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System.Collections.Generic;

namespace Innovt.Cloud.Table;
/// <summary>
/// Represents an item within a transaction for write operations.
/// </summary>
public class TransactionWriteItem
{
    /// <summary>
    /// Gets or sets the type of write operation in the transaction.
    /// </summary>
    public TransactionWriteOperationType OperationType { get; set; }
    /// <summary>
    /// Gets or sets the name of the table associated with the transaction item.
    /// </summary>
    public string TableName { get; set; }
    /// <summary>
    /// Gets or sets the condition expression for the transaction item.
    /// </summary>
    public string ConditionExpression { get; set; }

    /// <summary>
    ///  Only for update operations
    /// </summary>
    public string UpdateExpression { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
    /// <summary>
    /// Represents a dictionary of keys, typically used for key-value pairs.
    /// </summary>
    public Dictionary<string, object> Keys { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only


#pragma warning disable CA2227 // Collection properties should be read only
    /// <summary>
    /// Only for Put operations
    /// </summary>
    public Dictionary<string, object> Items { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only


#pragma warning disable CA2227 // Collection properties should be read only
    /// <summary>
    /// Represents a dictionary of attribute values to be used in expressions.
    /// </summary>
    public Dictionary<string, object> ExpressionAttributeValues { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
}