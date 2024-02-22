// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System.Collections.Generic;

namespace Innovt.Cloud.Table;

/// <summary>
///     Represents a response from executing an SQL statement, including a list of items of type T.
/// </summary>
/// <typeparam name="T">The type of items in the response.</typeparam>
public class ExecuteSqlStatementResponse<T> where T : class
{
    /// <summary>
    ///     Gets or sets a token that can be used for paginating results if necessary.
    /// </summary>
    public string NextToken { get; set; }

    /// <summary>
    ///     Gets or sets a list of items of type T returned by the SQL statement execution.
    /// </summary>
    public IList<T> Items { get; set; }
}