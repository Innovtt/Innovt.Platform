namespace Innovt.Data.QueryBuilders.Clause
{
    public class OrderByClause:ClauseAB
    {
        public string[] Columns { get; set; }
        public bool Ascending { get; set; }
        public OrderByClause(bool ascending,params string[] columns):base("ORDER")
        {
            this.Ascending = ascending;
            this.Columns = columns;
        }

    }
}