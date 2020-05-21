namespace Innovt.Data.QueryBuilders.Clause
{
    public class PaginationClause:ClauseAB
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public PaginationClause(int page,int pageSize):base("PAGINATION")
        {
            this.Page = page;
            this.PageSize = pageSize;
        }
    }
}