// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data.Ado
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Cqrs.Queries;
using Innovt.Core.Utilities;
using Innovt.Data.DataSources;
using Innovt.Data.Model;

namespace Innovt.Data.Ado
{
    internal static class Extensions
    {
        internal static string AddPagination(this string rawSql, IPagedFilter pagedFilter, IDataSource dataSource)
        {
            if (pagedFilter.IsNull())
                return rawSql;

            var recordStart = pagedFilter.Page * pagedFilter.PageSize;

            if (recordStart < 0)
                recordStart = 0;

            switch (dataSource.Provider)
            {
                case Provider.PostgreSqL:
                    return $"{rawSql} OFFSET ({recordStart}) LIMIT @PageSize ";
                case Provider.Oracle:
                    pagedFilter.Page = pagedFilter.Page <= 0 ? 1 : pagedFilter.Page;
                    return
                        $" SELECT * FROM (SELECT a.*, rownum r_  FROM ({rawSql}) a WHERE rownum < (({pagedFilter.Page} * {pagedFilter.PageSize}) + 1) ) WHERE r_ >=  ((({pagedFilter.Page} - 1) * {pagedFilter.PageSize}) + 1)";
                case Provider.MsSql:
                    return $"{rawSql} OFFSET {recordStart} ROWS FETCH NEXT @PageSize ROWS ONLY";
                default:
                    return $"{rawSql} OFFSET {recordStart} ROWS FETCH NEXT @PageSize ROWS ONLY";
            }
        }

        internal static string AddNoLock(this string rawSql,IDataSource dataSource)
        {
            return dataSource.Provider switch
            {
                Provider.PostgreSqL => rawSql,
                _ => rawSql + " WITH(NOLOCK) "
            };
        }

        internal static string AddWhere(this string rawSql,string whereClause)
        {
            return whereClause.IsNullOrEmpty() ? rawSql : $"{rawSql} WHERE {whereClause}";
        }
    }
}