using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Innovt.Core.Collections;
using Innovt.Core.Cqrs.Queries;
using Innovt.Data.Model;
using Innovt.Data.QueryBuilders.Clause;

namespace Innovt.Data.QueryBuilders
{
    public abstract class BaseQueryBuilder: IQueryBuilder
    {
        protected List<IClause> Clauses  = new List<IClause>();

        public bool UseNoLock { get; set; }
        
        public bool RespectColumnSyntax { get; set; }

        protected BaseQueryBuilder()
        {
            UseNoLock = true;
            RespectColumnSyntax = true;
        }

        internal IClause GetSingleClause(string name)
        {   
            var clause = Clauses.SingleOrDefault(c => c.Name == name);

            return clause;
        }
        private void RemoveClauseIfExist(string clauseName)
        {
            var fromClause = GetSingleClause(clauseName);

            Clauses?.Remove(fromClause);
        }

        private void AddOrReplaceClause(IClause clause)
        {
            RemoveClauseIfExist(clause.Name);

            Clauses.Add(clause);
        }

        public IQueryBuilder From(string tableName, string alias = null,bool useNoLock=false)
        {
            var fromClause = new FromClause(tableName, alias, useNoLock);

            AddOrReplaceClause(fromClause);

            return this;
        }


        public IQueryBuilder Select(params string[] columns)
        {
            if (!columns.IsNotNullOrEmpty())
                return this;

            var select = new SelectClause(columns);

            AddOrReplaceClause(select);
         
            return this;
        }

        public IQueryBuilder Top(int top)
        {
            var topClause = new TopClause(top);

            if (GetSingleClause(topClause.Name) is SelectClause selectClause)
            {
                topClause.Columns = selectClause.Columns;
            }

            AddOrReplaceClause(topClause);

            return this;
        }

        public virtual IQueryBuilder Count(bool distinct, params string[] columns)
        {
            var clause = new CountClause(distinct,columns);

            AddOrReplaceClause(clause);

            return this;
        }


        public virtual IQueryBuilder Count(params string[] columns)
        {
            return Count(false,columns);
        }
      

        public IQueryBuilder Where(string leftSide,string op,string rightSide)
        { 
            Clauses.Add(new WhereClause(leftSide,op,rightSide));

            return this;
        }

        public IQueryBuilder Where(string where)
        { 
            Clauses.Add(new WhereClause(where));

            return this;
        }

        public virtual IQueryBuilder OrderBy(params OrderBy[] orderBys)
        {
            if (orderBys == null)
                return this;

            foreach (var order in orderBys)
            {
                Clauses.Add(new OrderByClause(order.Ascending, order.Columns));
            }

            return this;
        }

        public IQueryBuilder Paginate(IPagedFilter filter)
        {
            var clause = new PaginationClause(filter.Page, filter.PageSize);
      
            AddOrReplaceClause(clause);

            return this;
        }

        public  IQueryBuilder FromRaw(string sql)
        {  

            return this;
        }
         
        public virtual string CompileSelect(SelectClause clause)
        {
            var sql = "SELECT";

            if (clause == null)
                return $"{sql} 1";

            var columns = $"{BuildColumns(clause.Columns)}";

            if (clause is CountClause countClause)
            {
                sql += " COUNT(";

                if (countClause.Distinct && columns.IsNotNullOrEmpty())
                {
                    sql += $"DISTINCT ({columns})";
                }
                else
                {
                    sql += "1)";
                }

                return sql;
            }

            if (clause is TopClause topClause)
            {
                sql += $" TOP {topClause.Limit}";

                if(columns.IsNotNullOrEmpty()){

                    sql += $" {columns}";
                }

                return sql;
            }

            return $"{sql} {columns}";
        }

  
        public virtual string CompileFrom(FromClause clause)
        {
            if (clause == null)
                return string.Empty;

            var from = $"{clause.Name} [{clause.Table}]" + (!clause.Alias.IsNullOrEmpty()
                ? $" AS {clause.Alias}"
                : string.Empty);

            from += (clause.UseNoLock || this.UseNoLock) ? " WITH(NOLOCK)" : string.Empty;

            return from;
        }

        public virtual string CompileWheres(IList<WhereClause> clauses)
        {
            if (clauses.IsNullOrEmpty())
                return string.Empty;

            var whereClause = new StringBuilder("WHERE ");

            foreach (var clause in clauses)
            {
                whereClause.Append(clause.Condition);
            }

            return whereClause.ToString();
        }

        public virtual string CompileOrderBy(IList<OrderByClause> clauses)
        {
            if (clauses == null)
                return string.Empty;

            var orderBy = new StringBuilder();

            foreach (var clause in clauses)
            {
                var columns = BuildColumns(clause.Columns);

                if (orderBy.Length == 0)
                {
                    orderBy.Append($"ORDER BY {columns}");
                }
                else
                {
                    orderBy.Append($",{orderBy}");
                }

                orderBy.Append(clause.Ascending ? " ASC" : " DESC");
            }

            return orderBy.ToString();
        }

        protected abstract string BuildColumns(string[] columns);

        public abstract string CompilePagination(PaginationClause clause);


        public string Sql()
        {  
            var select = Clauses.SingleOrDefault(c=> c.Name == "SELECT") as SelectClause;

            var from = Clauses.SingleOrDefault(c => c.Name == "FROM") as FromClause;

            var wheres = Clauses.Where(c => c.Name =="WHERE").Cast<WhereClause>().ToList();

            var orderBys = Clauses.Where(c=> c.Name=="ORDER").Cast<OrderByClause>().ToList();

            var pagination = Clauses.SingleOrDefault(c=> c.Name == "PAGINATION") as PaginationClause;

            var results = new[] {
                    this.CompileSelect(select),
                    this.CompileFrom(from),
                    this.CompileWheres(wheres),
                    this.CompileOrderBy(orderBys),
                    this.CompilePagination(pagination)
                }
                .Where(x => x != null)
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            var sql = string.Join(" ", results);

            return sql;
        }
    }
}