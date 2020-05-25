using Dapper;
using Innovt.Core.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Innovt.Core.Cqrs.Queries;
using System.Threading;
using Innovt.Data.DataSources;
using Innovt.Data.Exceptions;
using Innovt.Data.SqlKata;
using JetBrains.Annotations;
using SqlKata;

namespace Innovt.Data.Ado
{
    public class RepositoryBase
    {
        private readonly IDataSource datasource;
        private readonly IConnectionFactory connectionFactory;

        public RepositoryBase(IDataSource datasource) : this(datasource, null)
        {
        }

        public RepositoryBase(IDataSource datasource, IConnectionFactory connectionFactory)
        {
            this.datasource = datasource ?? throw new ArgumentNullException(nameof(datasource));

            this.connectionFactory = connectionFactory ?? new ConnectionFactory();
        }

        protected IDbConnection GetConnection()
        {
            return connectionFactory.Create(datasource);
        }

        protected Query CreateQuery(string tableName = "")
        {
            return new Query(tableName);
        }

        internal async Task<T> QueryFirstOrDefaultInternalAsync<T>([NotNull] Query query, CancellationToken cancellationToken = default)
        {
            var compiled = query.Compile(datasource);

            using var con = GetConnection();

            var result = await con.QueryFirstOrDefaultAsync<T>(new CommandDefinition(compiled.Sql,
                compiled.NamedBindings, cancellationToken: cancellationToken)).ConfigureAwait(false);

            return result;
        }

        protected Task<T> QueryFirstOrDefaultAsync<T>([NotNull] Query query, CancellationToken cancellationToken = default) where T : class
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return QueryFirstOrDefaultInternalAsync<T>(query, cancellationToken);
        }

        internal async Task<T> QueryFirstInternalAsync<T>([NotNull] Query query,
            CancellationToken cancellationToken = default)
        {
            var compiled = query.Compile(datasource);

            using var con = GetConnection();

            var result = await con
                .QueryFirstAsync<T>(new CommandDefinition(compiled.Sql,compiled.NamedBindings,
                    cancellationToken: cancellationToken)).ConfigureAwait(false);

            return result;
        }

        protected Task<T> QueryFirstAsync<T>([NotNull] Query query, CancellationToken cancellationToken = default) where T : class
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return QueryFirstInternalAsync<T>(query, cancellationToken);
        }

        internal async Task<T> QuerySingleOrDefaultInternalAsync<T>(Query query, CancellationToken cancellationToken = default) where T : class
        {
            var compiled = query.Compile(datasource);

            using var con = GetConnection();

            return await con.QuerySingleOrDefaultAsync<T>(new CommandDefinition(compiled.Sql,compiled.NamedBindings, cancellationToken: cancellationToken)).ConfigureAwait(false);
        }

        protected Task<T> QuerySingleOrDefaultAsync<T>([NotNull] Query query, CancellationToken cancellationToken = default) where T : class
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return QuerySingleOrDefaultInternalAsync<T>(query, cancellationToken);
        }

        //protected Task<T> QuerySingleOrDefaultAsync<T>([NotNull] string tableName, [NotNull] string whereClause, object parameters = null, CancellationToken cancellationToken = default,
        //    params string[] columns) where T : class
        //{
        //    if (tableName == null) throw new ArgumentNullException(nameof(tableName));
        //    if (whereClause == null) throw new ArgumentNullException(nameof(whereClause));

        //    var query = this.CreateQuery(tableName).WhereRaw(whereClause).Select(columns);


        //    return this.QuerySingleOrDefaultInternalAsync<T>(query, parameters, cancellationToken);
        //}

        internal async Task<T> QuerySingleInternalAsync<T>(Query query, CancellationToken cancellationToken = default)
        {
            var compiled = query.Compile(datasource);

            using var con = GetConnection();

            return await con.QuerySingleAsync<T>(new CommandDefinition(compiled.Sql,compiled.NamedBindings, cancellationToken: cancellationToken)).ConfigureAwait(false);
        }

        protected Task<T> QuerySingleAsync<T>([NotNull] Query query, CancellationToken cancellationToken = default)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return QuerySingleInternalAsync<T>(query, cancellationToken);
        }

        internal async Task<int> QueryCountInternalAsync(string tableName, string whereClause = null, CancellationToken cancellationToken = default)
        {
            var query = this.CreateQuery(tableName).WhereRaw(whereClause).AsCount();

            return await this.QuerySingleInternalAsync<int>(query, cancellationToken).ConfigureAwait(false);
        }

        protected Task<int> QueryCountAsync([NotNull] string tableName, string whereClause = null, CancellationToken cancellationToken = default)
        {
            if (tableName == null) throw new ArgumentNullException(nameof(tableName));

            return QueryCountInternalAsync(tableName, whereClause, cancellationToken);
        }

        internal async Task<IEnumerable<T>> QueryInternalAsync<T>([NotNull] Query query, CancellationToken cancellationToken = default) where T : class
        {
            var compiled = query.Compile(datasource);

            using var con = GetConnection();

            var result = await con.QueryAsync<T>(new CommandDefinition(compiled.Sql, compiled.NamedBindings, cancellationToken: cancellationToken)).ConfigureAwait(false);

            return result;
        }

        protected Task<IEnumerable<T>> QueryAsync<T>([NotNull] Query query, CancellationToken cancellationToken = default) where T : class
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return QueryInternalAsync<T>(query, cancellationToken);
        }

        internal async Task<PagedCollection<T>> QueryPagedInternalAsync<T>([NotNull] Query query,
            [NotNull] IPagedFilter filter, CancellationToken cancellationToken = default) where T : class
        {   
            var compiled = query.Compile(datasource);

            var orderByIndex = compiled.Sql.LastIndexOf("ORDER BY", StringComparison.CurrentCultureIgnoreCase);

            if (orderByIndex <= 0)
                throw new SqlSyntaxException("ORDER BY Clause not found. To filter as paged you need to provide an ORDER BY Clause.");

            var sql = $"SELECT COUNT(1) FROM ({compiled.Sql.Substring(0, orderByIndex)}) C;";

            compiled = query.Clone().ForPage(filter.Page, filter.PageSize).Compile(datasource);

            sql += compiled.Sql;

            using var con = GetConnection();

            var qResult = await con.QueryMultipleAsync(new CommandDefinition(sql, compiled.NamedBindings, cancellationToken: cancellationToken)).ConfigureAwait(false);

            var totalRecords = qResult.ReadFirst<int>();

            var result = new PagedCollection<T>(qResult.Read<T>())
            {
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalRecords = totalRecords
            };
            return result;
        }

        protected Task<PagedCollection<T>> QueryPagedAsync<T>([NotNull] Query query, [NotNull] IPagedFilter filter, CancellationToken cancellationToken = default) where T : class
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (query == null) throw new ArgumentNullException(nameof(query));

            return QueryPagedInternalAsync<T>(query, filter, cancellationToken);
        }

        internal async Task<IEnumerable<T>> QueryListPagedInternalAsync<T>([NotNull] Query query, [NotNull] IPagedFilter filter, CancellationToken cancellationToken = default)
        {
            var pagedQuery =  query.Clone().ForPage(filter.Page,filter.PageSize);
            
            var compiled = pagedQuery.Compile(datasource);

            using var con = GetConnection();

            return  await con.QueryAsync<T>(new CommandDefinition(compiled.Sql, compiled.NamedBindings, cancellationToken: cancellationToken)).ConfigureAwait(false);
        }

        protected Task<IEnumerable<T>> QueryListPagedAsync<T>([NotNull] Query query, [NotNull] IPagedFilter filter, CancellationToken cancellationToken = default)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (query == null) throw new ArgumentNullException(nameof(query));

            return QueryListPagedInternalAsync<T>(query, filter, cancellationToken);
        }

        internal async Task<SqlMapper.GridReader> QueryMultipleInternalAsync(Query[] queries)
        {
            var sql =new StringBuilder();
            var nameBindings = new Dictionary<string,object>();

            foreach (var query in queries)
            {
                var compiled = query.Compile(datasource);
                sql.Append(compiled.Sql);

               // nameBindings.add.AddFluent(compiled.NamedBindings);
            }

            using var con = GetConnection();

           return  await con.QueryMultipleAsync(sql.ToString(), nameBindings).ConfigureAwait(false);
        }

        internal async Task<IEnumerable<TReturn>> QueryMultipleInternalAsync<TFirst, TSecond, TReturn>(Query[] queries, Func<TFirst, TSecond, TReturn> func, string splitOn = "id")
        {
            var result = await QueryMultipleInternalAsync(queries);
            return result.Read(func, splitOn, true).ToList();
        }

        protected Task<IEnumerable<TReturn>> QueryMultipleAsync<TFirst, TSecond, TReturn>(Query[] queries, [NotNull] Func<TFirst, TSecond, TReturn> func, string splitOn = "id")
        {
            if (queries == null) throw new ArgumentNullException(nameof(queries));
            if (func == null) throw new ArgumentNullException(nameof(func));

            return QueryMultipleInternalAsync(queries, func,splitOn);
        }
    }
}
