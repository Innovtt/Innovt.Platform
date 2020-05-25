using System.Collections.Generic;
using System.Linq;
using Innovt.Core.Collections;
using Innovt.Data.DataSources;
using SqlKata;

namespace Innovt.Data.SqlKata
{
    public static class Extensions
    {
        public static (string Sql,Dictionary<string,object> NamedBindings) Compile(this Query query,IDataSource dataSource)
        {
            var result = CompilerFactory.Create(dataSource).Compile(query);

            return (result.Sql, result.NamedBindings);
        }

        /// <summary>
        /// This helper 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static Query AddOrderBy(this Query query, string orderBy)
        {
            if (orderBy.IsNullOrEmpty())
                return query;

            const string descOperator = "-";
    
            var orderOperator = orderBy.First().ToString();
            var order = orderBy.Substring(1, orderBy.Length - 1);

            return orderOperator switch
            {
                descOperator => query.OrderByDesc(order),
                _ => query.OrderBy(order)
            };
        }

        public static Query AddOrderBy(this Query query,ParamsWrappers<Innovt.Data.Model.OrderBy> orderBys)
        {
            if(orderBys==null)
                return query;

            foreach (var order in orderBys.Parameters)
            {
                if (order.Ascending)
                {
                    query.OrderBy(order.Columns);
                }
                else
                {
                    query.OrderByDesc(order.Columns);
                }
            }

            return query;
        }
    }
}