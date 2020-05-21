using Dapper;
using Innovt.Core.Collections;
using Innovt.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Queries;
using System.Threading;
using Innovt.Data.DataSources;
using Innovt.Data.Exceptions;
using Innovt.Data.QueryBuilders;
using Innovt.Data.SqlKata;
using SqlKata;
using OrderBy = Innovt.Data.Model.OrderBy;

namespace Innovt.Data.Ado
{
    public class RepositoryBase
    {
        private readonly IDataSource datasource;
        private readonly IConnectionFactory connectionFactory;

        public IQueryBuilder QueryBuilder { get; private set; }

        public RepositoryBase(IDataSource datasource):this(datasource,default(IQueryBuilder),null)
        {
        }
        public RepositoryBase(IDataSource datasource, IQueryBuilder queryBuilder):this(datasource,queryBuilder,null)
        {
            this.datasource = datasource ?? throw new ArgumentNullException(nameof(datasource));

            QueryBuilder = queryBuilder ?? QueryBuilderFactory.Create(datasource);
        }

        public RepositoryBase(IDataSource datasource,IQueryBuilder queryBuilder, IConnectionFactory connectionFactory)
        {
            this.datasource = datasource ?? throw new ArgumentNullException(nameof(datasource));
            
            QueryBuilder = queryBuilder ?? QueryBuilderFactory.Create(datasource);

            this.connectionFactory = connectionFactory ?? new ConnectionFactory();
        }

        protected IDbConnection GetConnection()
        {
            return connectionFactory.Create(datasource);
        }

        protected string RenderSql(Query query)
        {
            return query?.RenderSql(datasource);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string tableName, string whereClause,ParamsWrappers<OrderBy> orderBy, 
            object filter = null, CancellationToken cancellationToken = default,params string[] columns) where T : class
        {
            var sql = new Query(tableName).Select(columns).Where(whereClause).AddOrderBy(orderBy).Limit(1)
                .RenderSql(datasource);

            using var con = GetConnection();

            var result = await con.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken));

            return result;
        }


        public async Task<int> QuerySingleAsync(string sql, object filter = null, CancellationToken cancellationToken = default)
        {
            var query = new Query().FromRaw(sql).RenderSql(datasource);

            using var con = GetConnection();

            return await con.QuerySingleAsync<int>(new CommandDefinition(query, filter, cancellationToken: cancellationToken));
        }

        public async Task<int> QueryCountAsync(string tableName, string whereClause = null, object filter = null, CancellationToken cancellationToken = default)
        {
            var sql = QueryBuilder.Count().From(tableName).Where(whereClause).Sql();
          
            return await this.QuerySingleAsync(sql, filter,cancellationToken);
        }
    
        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object filter = null, CancellationToken cancellationToken = default) where T : class
        {
            Check.NotNull(sql, nameof(sql));

            using var con = GetConnection();

            var result = await con.QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken));

            return result;
        }
     
    
        public async Task<T> QuerySingleOrDefaultAsync<T>(string tableName, string whereClause, object filter = null, CancellationToken cancellationToken = default, params string[] columns) where T : class
        {
            var sql = QueryBuilder.Select(columns).From(tableName).Where(whereClause).Sql();

            return await this.QuerySingleOrDefaultAsync<T>(sql, filter, cancellationToken);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(Query query, object filter = null, CancellationToken cancellationToken = default) where T : class
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            using var con = GetConnection();

            var result = await con.QueryAsync<T>(new CommandDefinition(query.RenderSql(datasource), filter, cancellationToken: cancellationToken));

            return result;
        }


        //public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object filter=null, CancellationToken cancellationToken = default) where T : class
        //{
        //    using var con = GetConnection();

        //    var result = await con.QueryAsync<T>(new CommandDefinition(sql, filter, cancellationToken: cancellationToken));

        //    return result;
        //}

        public async Task<IEnumerable<T>> QueryAsync<T>(string tableName, string whereClause, object filter = null,ParamsWrappers<OrderBy> orderBys = null,
            CancellationToken cancellationToken = default,params string[] columns) where T : class
        {
            var sql = new Query(tableName).Select(columns).orde
                
                .Select(columns).From(tableName).Where(whereClause).OrderBy(orderBys.Parameters).Sql();

            return await this.QueryAsync<T>(sql, filter,cancellationToken);
        }


        public async Task<PagedCollection<T>> QueryPagedAsync<T>(string sql, IPagedFilter filter, CancellationToken cancellationToken = default) where T : class
        {
            Check.NotNull(filter, nameof(filter));

            var orderByIndex = sql.LastIndexOf("ORDER BY", StringComparison.CurrentCultureIgnoreCase);

            if (orderByIndex <= 0)
                throw new SqlSyntaxException("ORDER BY Clause not found. To filter as paged you need to provide an ORDER BY Clause.");
            
            var newSql = sql.Substring(0, orderByIndex);

            var query  = $"SELECT COUNT(1) FROM ({newSql}) C;" + sql;

            using var con = GetConnection();

            var qResult = await con.QueryMultipleAsync(new CommandDefinition(query, filter, cancellationToken: cancellationToken));

            var totalRecords = qResult.ReadFirst<int>();

            var result = new PagedCollection<T>(qResult.Read<T>())
            {
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalRecords = totalRecords
            };
            return result;
        }

        public async Task<PagedCollection<T>> QueryPagedAsync<T>(string tableName, string whereClause, IPagedFilter filter,ParamsWrappers<OrderBy> orderBys, CancellationToken cancellationToken = default,  params string[] columns) where T : class
        {
            Check.NotNull(filter, nameof(filter));

            var sql = QueryBuilder.Select(columns).From(tableName).Where(whereClause).OrderBy(orderBys.Parameters).Sql();

            return await QueryPagedAsync<T>(sql, filter,cancellationToken:cancellationToken);
        }
    }
}
