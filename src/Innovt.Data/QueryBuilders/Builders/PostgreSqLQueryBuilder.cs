using System;
using System.Collections.Generic;
using Innovt.Core.Collections;
using Innovt.Core.Utilities;
using Innovt.Data.QueryBuilders.Clause;

namespace Innovt.Data.QueryBuilders.Builders
{
    public class PostgreSqlQueryBuilder : BaseQueryBuilder
    {
        public PostgreSqlQueryBuilder(bool respectColumnSyntax = true)
        {
            RespectColumnSyntax = respectColumnSyntax;
            UseNoLock = false;
        }

        protected override string BuildColumns(string[] columns)
        {
            if (columns.IsNullOrEmpty())
                return string.Empty;

            var fields = new List<string>();

            foreach (var column in columns)
            {  
                var tableAlias = string.Empty;
                var field = column;

                if (column.Contains(".", StringComparison.InvariantCultureIgnoreCase))
                {
                    //checking alias
                    tableAlias = column.Split('.')[0] +".";
                    field = column.Split('.')[1];
                }

                if (RespectColumnSyntax)
                {
                    field = @"""" + $"{field}"+ @"""";
                }
                else
                {
                    field = field.ToLower();
                }

                fields.Add($"{tableAlias}{field}");
            }

            return string.Join(",", fields);
        }
        public override string CompilePagination(PaginationClause clause)
        {
            if (clause.IsNull())
                return string.Empty;

            var recordStart = (clause.Page - 1) * clause.PageSize;
    
            return $"OFFSET ({recordStart}) LIMIT {clause.PageSize}";
        }
    }
}