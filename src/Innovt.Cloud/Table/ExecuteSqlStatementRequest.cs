// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

namespace Innovt.Cloud.Table;

/// <summary>
///     Represents a request to execute an SQL statement.
/// </summary>
public class ExecuteSqlStatementRequest
{
    /// <summary>
    ///     Gets or sets a flag indicating whether to perform a consistent read.
    /// </summary>
    public bool ConsistentRead { get; set; }

    /// <summary>
    ///     Gets or sets the SQL statement to be executed.
    /// </summary>
    public string Statment { get; set; }

    /// <summary>
    ///     Gets or sets a token that can be used for paginating results if necessary.
    /// </summary>
    public string NextToken { get; set; }
}