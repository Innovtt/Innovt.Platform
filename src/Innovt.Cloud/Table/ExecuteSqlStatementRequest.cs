namespace Innovt.Cloud.Table;

public class ExecuteSqlStatementRequest
{
    public bool ConsistentRead { get; set; }
    public string Statment { get; set; }
    public string NextToken { get; set; }
}