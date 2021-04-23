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

            var recordStart  = (pagedFilter.Page) * pagedFilter.PageSize;

            if (recordStart < 0 )
                recordStart = 0;

            return dataSource.Provider switch
            {
                Provider.PostgreSqL => $"{rawSql} OFFSET ({recordStart}) LIMIT @PageSize ",
                Provider.Oracle =>
                    $" SELECT * FROM (SELECT a.*, rownum r_  FROM ({rawSql}) a WHERE rownum < (({pagedFilter.Page} * {pagedFilter.PageSize}) + 1) ) WHERE r_ >=  ((({pagedFilter.Page} - 1) * {pagedFilter.PageSize}) + 1)",
                Provider.MsSql => $"{rawSql} OFFSET {recordStart} ROWS FETCH NEXT @PageSize ROWS ONLY",
                _ => $"{rawSql} OFFSET {recordStart} ROWS FETCH NEXT @PageSize ROWS ONLY"
            };
        }
    }
}