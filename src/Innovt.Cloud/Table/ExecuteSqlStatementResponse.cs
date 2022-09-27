// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud

using System.Collections.Generic;

namespace Innovt.Cloud.Table;

public class ExecuteSqlStatementResponse<T> where T : class
{
    public string NextToken { get; set; }

    public IList<T> Items { get; set; }
}