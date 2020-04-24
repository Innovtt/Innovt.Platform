using System;
using Innovt.Core.Cqrs.Queries;
using Innovt.Core.Utilities;

namespace Innovt.Data
{
    public static class Extensions
    {
        public static string ApplyPagination(this string sql, IPagedFilter filterBase)
        {
            if (filterBase.IsNull())
                return sql;
            

            if (filterBase.PageSize<=0)
            {
                filterBase.PageSize = 10;
            }

            var recordStart = (filterBase.Page) * filterBase.PageSize;

            sql += $" OFFSET {recordStart} ROWS FETCH NEXT {filterBase.PageSize} ROWS ONLY";

            return sql;
        }
    }
}
