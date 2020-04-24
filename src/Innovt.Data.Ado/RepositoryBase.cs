using Dapper;
using Innovt.Core.Collections;
using Innovt.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Queries;
using System.Threading;

namespace Innovt.Data.Ado
{
    public class RepositoryBase
    {
        private readonly string connectionString;

        public RepositoryBase(IDataSource datasource)
        {   
            connectionString = datasource?.GetConnectionString() ?? throw new ArgumentNullException(nameof(datasource));
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Will get the first item or null
        /// </summary>
        /// <typeparam name="T">The generic type</typeparam>
        /// <param name="tableName">the table name</param>
        /// <param name="whereClause">the where clouse, in this case just 1=1 or A=@a</param>
        /// <param name="filter">The filter object</param>
        /// <param name="columns">You need to provide all collumuns that will be returned.</param>
        /// <returns></returns>
        public async Task<T> QueryFirstOrDefaultAsync<T>(string tableName, string whereClause, object filter = null, CancellationToken cancellationToken = default,params string[] columns) where T : class
        {
            var fields = "*";

            if (columns != null && columns.Any())
                fields = string.Join(",", columns);


            var sql = $"SELECT TOP 1 {fields} FROM [{tableName}] WITH(NOLOCK) WHERE {whereClause}";

            using var con = GetConnection();
            var result = await con.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken));
            return result;
        }
        
        public async Task<int> QuerySingleAsync(string sql, object filter = null, CancellationToken cancellationToken = default)
        {
            using var con = GetConnection();
            return await con.QuerySingleAsync<int>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken));
        }

        public async Task<int> QueryCountAsync(string tableName, string whereClause = null, object filter = null, CancellationToken cancellationToken = default)
        {
            var sql = $"SELECT COUNT(1) FROM [{tableName}] WITH(NOLOCK) ";

            if (whereClause.IsNotNullOrEmpty())
            {
                sql += $" WHERE { whereClause} ";
            }

            return await this.QuerySingleAsync(sql, filter,cancellationToken);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type you want</typeparam>
        /// <param name="sql">The sql query </param>
        /// <param name="filter">Some filter </param>
        /// <param name="cancellationToken"></param>
        /// <returns>Only oone element </returns>
        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object filter = null, CancellationToken cancellationToken = default) where T : class
        {
            Check.NotNull(sql, nameof(sql));

            using var con = GetConnection();
            var result = await con.QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken));
            return result;
        }
     
        /// <summary>
        /// You execute a simple query and get only one element
        /// </summary>
        /// <typeparam name="T">The return Type</typeparam>
        /// <param name="tableName">Table Name</param>
        /// <param name="whereClause"> You don't have to add WHERE a=@a, only a=@a.</param>
        /// <param n ame="filter"></param>
        /// <param name="columns">All colluns that you want to be returned.</param>
        /// <returns></returns>
        public async Task<T> QuerySingleOrDefaultAsync<T>(string tableName, string whereClause, object filter = null, CancellationToken cancellationToken = default, params string[] columns) where T : class
        {
            var sql = $"SELECT {string.Join(",", columns)} FROM [{tableName}] WITH(NOLOCK) WHERE {whereClause}";

            return await this.QuerySingleOrDefaultAsync<T>(sql, filter, cancellationToken);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object filter=null, CancellationToken cancellationToken = default) where T : class
        {
            using var con = GetConnection();
            var result = await con.QueryAsync<T>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken));

            return result;
        }
        

        public async Task<IEnumerable<T>> QueryAsync<T>(string tableName, string whereClause, object filter = null, string orderBy = null, CancellationToken cancellationToken = default,params string[] columns) where T : class
        {
            var sql = $"SELECT {string.Join(",", columns)} FROM [{tableName}] WITH(NOLOCK) WHERE {whereClause}";

            if (orderBy.IsNotNullOrEmpty())
            {
                sql += " ORDER BY " + orderBy;
            }

            return await this.QueryAsync<T>(sql, filter,cancellationToken);
        }

        /// <summary>
        /// QueryPaggedAsync Will returna always a paginated result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="filter"></param>
        /// <param name="automaticQueryCount">Is true we will generate a Select COUNT(1) FROM ... based on your current query</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedCollection<T>> QueryPagedAsync<T>(string sql, IPagedFilter filter, bool automaticQueryCount = true, CancellationToken cancellationToken = default) where T : class
        {
            Check.NotNull(filter, nameof(filter));

            var orderByIndex = sql.LastIndexOf("ORDER BY", StringComparison.CurrentCultureIgnoreCase);

            if (orderByIndex <= 0)
                throw new Exception("ORDER BY Clause not found. To filter as pagged you need to provide an ORDER BY Clause.");
            
            if (automaticQueryCount)
            {
                //removing order by
                var newSql = sql.Substring(0, orderByIndex);

                sql = $"SELECT COUNT(1) FROM ({newSql}) C;" + sql;
            }

            sql = sql.ApplyPagination(filter);

            using var con = GetConnection();
            var qResult = await con.QueryMultipleAsync(new CommandDefinition(sql, filter, cancellationToken: cancellationToken));

            int totalRecords = 0;

            if (automaticQueryCount)
            {
                totalRecords = qResult.ReadFirst<int>();
            }

            var result = new PagedCollection<T>(qResult.Read<T>())
            {
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalRecords = totalRecords
            };
            return result;
        }

        public async Task<PagedCollection<T>> QueryPagedAsync<T>(string tableName, string whereClause, IPagedFilter filter, string orderBy, CancellationToken cancellationToken = default,  params string[] columns) where T : class
        {
            Check.NotNull(filter, nameof(filter));

            var sql = $" SELECT {string.Join(",", columns)} FROM [{tableName}] WITH(NOLOCK) WHERE {whereClause} ";
            
            sql += " ORDER BY " + orderBy;
            

            return await QueryPagedAsync<T>(sql, filter,cancellationToken:cancellationToken);
        }

        public void ThrowExceptionIfFilterIsInvalid(IPagedFilter filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
        }

        public void ThrowExceptionIfFilterIsInvalid(SimpleFilter<int> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            if (filter.Data <= 0) throw new ArgumentNullException(nameof(filter.Data));
        }

        public void ThrowExceptionIfFilterIsInvalid(int id)
        {
            if (id <= 0) throw new ArgumentNullException(nameof(id));
        }
    }
}
