// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

namespace Innovt.Cloud.Table;
/// <summary>
/// Defines the types of write operations within a transaction.
/// </summary>
public enum TransactionWriteOperationType
{
    /// <summary>
    /// Represents a "Put" operation in the transaction.
    /// </summary>
    Put = 0,

    /// <summary>
    /// Represents an "Update" operation in the transaction.
    /// </summary>
    Update = 1,

    /// <summary>
    /// Represents a "Delete" operation in the transaction.
    /// </summary>
    Delete = 2,

    /// <summary>
    /// Represents a "Check" operation in the transaction.
    /// </summary>
    Check = 3
}