
namespace Innovt.Data.QueryBuilders.Clause
{
    public interface ISelectClause: IClause
    {
        public string[] Columns { get; set; }
    }
}