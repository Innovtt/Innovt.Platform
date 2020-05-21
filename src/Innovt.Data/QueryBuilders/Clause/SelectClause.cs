using Innovt.Core.Collections;

namespace Innovt.Data.QueryBuilders.Clause
{
    public class SelectClause: ClauseAB, ISelectClause
    {
        public string[] Columns { get; set; }

        public SelectClause():base("SELECT")
        {   
        }

        public SelectClause(params string[] columns):this()
        {
            if(columns.IsNullOrEmpty())
                return;

            Columns = columns;
        }

    }
}