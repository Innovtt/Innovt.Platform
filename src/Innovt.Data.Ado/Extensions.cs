// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data.Ado

using Innovt.Core.Cqrs.Queries;
using Innovt.Core.Utilities;
using Innovt.Data.DataSources;
using Innovt.Data.Model;

namespace Innovt.Data.Ado;
/// <summary>
/// Provides extension methods for SQL query manipulation.
/// </summary>
internal static class Extensions
{
    /// <summary>
    /// Adds pagination clauses to the raw SQL query based on the specified paged filter and data source.
    /// </summary>
    /// <param name="rawSql">The raw SQL query to modify.</param>
    /// <param name="pagedFilter">The paged filter containing pagination parameters.</param>
    /// <param name="dataSource">The data source used to determine the database provider.</param>
    /// <returns>The modified SQL query with pagination clauses.</returns>
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
    /// <summary>
    /// Adds a NOLOCK hint to the raw SQL query based on the specified data source.
    /// </summary>
    /// <param name="rawSql">The raw SQL query to modify.</param>
    /// <param name="dataSource">The data source used to determine the database provider.</param>
    /// <returns>The modified SQL query with the NOLOCK hint.</returns>
    internal static string AddNoLock(this string rawSql, IDataSource dataSource)
    {
        return dataSource.Provider switch
        {
            Provider.PostgreSqL => rawSql,
            _ => rawSql + " WITH (NOLOCK) "
        };
    }
    /// <summary>
    /// Adds a WHERE clause to the raw SQL query based on the specified WHERE clause string.
    /// </summary>
    /// <param name="rawSql">The raw SQL query to modify.</param>
    /// <param name="whereClause">The WHERE clause to append.</param>
    /// <returns>The modified SQL query with the WHERE clause.</returns>
    internal static string AddWhere(this string rawSql, string whereClause)
    {
        return whereClause.IsNullOrEmpty() ? rawSql : $"{rawSql} WHERE {whereClause}";
    }
}