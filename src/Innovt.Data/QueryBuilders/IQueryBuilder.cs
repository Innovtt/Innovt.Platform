using Innovt.Core.Cqrs.Queries;
using Innovt.Data.Model;

namespace Innovt.Data.QueryBuilders
{
    public interface IQueryBuilder
    {
        bool UseNoLock { get; set; }
        bool RespectColumnSyntax { get; set; }
        IQueryBuilder From(string tableName, string alias = null,bool useNoLock=false);
        IQueryBuilder Select(params string[] columns);
        IQueryBuilder Top(int top);
        IQueryBuilder Count(params string[] columns);
        IQueryBuilder Count(bool distinct,params string[] columns);
        IQueryBuilder Where(string leftSide,string op,string rightSide);
        IQueryBuilder Where(string where);
        IQueryBuilder OrderBy(params OrderBy[] orderBys);
        IQueryBuilder Paginate(IPagedFilter filter);
        IQueryBuilder FromRaw(string sql);

        string Sql();
    }
}