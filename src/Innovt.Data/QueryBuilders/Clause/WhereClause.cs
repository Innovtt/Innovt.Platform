

namespace Innovt.Data.QueryBuilders.Clause
{
    public class WhereClause:ClauseAB
    {
        public string Condition { get; set; }

        public WhereClause(string leftSide,string op,string rightSide):this($"{leftSide} {op} {rightSide}")
        {
          
        }

        public WhereClause(string condition):base("WHERE")
        {
            Condition = condition;
        }
    }
}