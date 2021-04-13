// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Data.Ado
// Solution: Innovt.Platform
// Date: 2021-04-08
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

            return dataSource.Provider switch
            {
                Provider.PostgreSqL => $"{rawSql} OFFSET ({recordStart}) LIMIT @PageSize ",
                _ => $"{rawSql} OFFSET {recordStart} ROWS FETCH NEXT @PageSize ROWS ONLY"
            };
        }
    }
}