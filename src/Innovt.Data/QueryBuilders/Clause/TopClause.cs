namespace Innovt.Data.QueryBuilders.Clause
{
    public class TopClause:SelectClause, ISelectClause
    {
        public int Limit { get; set; }
        public TopClause(int limit):base()
        {
            Limit = limit;
        }
    }
}