
namespace Innovt.Cloud.Table
{
    public interface ITableMessage
    {
        string Id { get; set; }
        string RangeKey { get; set; }
    }
}