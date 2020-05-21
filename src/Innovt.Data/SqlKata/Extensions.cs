using Innovt.Core.Collections;
using Innovt.Data.DataSources;
using SqlKata;
using OrderBy = Innovt.Data.Model.OrderBy;

namespace Innovt.Data.SqlKata
{
    public static class Extensions
    {
        public static string RenderSql(this Query query,IDataSource dataSource)  
        {
            var compiler = CompilerFactory.Create(dataSource).Compile(query);
            return compiler.Sql;
        }

        public static Query AddOrderBy(this Query query,ParamsWrappers<OrderBy> orderBys)
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