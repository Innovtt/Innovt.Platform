namespace Innovt.Data.QueryBuilders.Clause
{
    public class FromClause:ClauseAB
    {
        public string Table { get; set; }

        public  string Alias { get; set; }

        public bool UseNoLock { get; set; }


        public FromClause():base("FROM")
        {   
        }

        public FromClause(string table,string alias=null,bool useNoLock=false):this()
        {
            Table = table;
            Alias = alias;
            UseNoLock = useNoLock;
        }

    }
}