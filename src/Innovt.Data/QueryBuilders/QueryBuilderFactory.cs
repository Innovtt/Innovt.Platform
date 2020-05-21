using System;
using Innovt.Data.DataSources;
using Innovt.Data.Model;
using Innovt.Data.QueryBuilders.Builders;

namespace Innovt.Data.QueryBuilders
{
    public static class QueryBuilderFactory
    {
        public static IQueryBuilder Create(IDataSource dataSource)
        {
            if (dataSource == null) throw new ArgumentNullException(nameof(dataSource));

            return dataSource.Provider switch
            {
                Provider.MsSql => new MsSqlQueryBuilder(),
                Provider.PostgreSqL => new PostgreSqlQueryBuilder(),
                _ => throw new NotImplementedException()
            };
        }
    }

}