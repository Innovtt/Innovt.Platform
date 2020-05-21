using System.Linq;
using Innovt.Core.Utilities;
using Innovt.Data.QueryBuilders.Clause;

namespace Innovt.Data.QueryBuilders.Builders
{
    public class MsSqlQueryBuilder : BaseQueryBuilder
    {
        public MsSqlQueryBuilder()
        {
            UseNoLock = true;
            RespectColumnSyntax = true;
        }

        protected override string BuildColumns(string[] columns)
        {
            if (columns.IsNull())
                return string.Empty;

            var formattedColumns = columns.Select(c => RespectColumnSyntax ? c : c.ToUpper());

            return string.Join(",", formattedColumns);
        }

        public override string CompilePagination(PaginationClause clause)
        {
            if (clause.IsNull())
                return string.Empty;

            var recordStart = (clause.Page) * clause.PageSize;

            return $"OFFSET {recordStart} ROWS FETCH NEXT {clause.PageSize} ROWS ONLY";
        }
    }
}