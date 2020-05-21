namespace Innovt.Data.QueryBuilders.Clause
{
    public class CountClause:SelectClause
    {
        public bool Distinct { get; set; }

        public CountClause(params string[] columns):base(columns)
        {
       
        }

        public CountClause()
        {
            
        }

        public CountClause(bool distinct,params string[] columns):base(columns)
        {
            Distinct = distinct;
        }
    }
}