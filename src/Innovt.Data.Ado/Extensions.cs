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

            int recordStart;
            switch (dataSource.Provider)
            {   case Provider.PostgreSqL:
                    recordStart = (pagedFilter.Page - 1) * pagedFilter.PageSize;

                    return $"{rawSql} OFFSET ({recordStart}) LIMIT @PageSize ";

                default:
                    recordStart = (pagedFilter.Page) * pagedFilter.PageSize;

                    return $"{rawSql} OFFSET {recordStart} ROWS FETCH NEXT @PageSize ROWS ONLY";
            }
        }
    }
}