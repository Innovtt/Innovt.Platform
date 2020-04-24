using System.Collections.Generic;

namespace Innovt.Core.Collections
{
    public class PagedCollection<T> : IPagedCollection<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }

        public PagedCollection(IEnumerable<T> collection,int? page,int? pageSize)
        {
            this.Items = collection;
            this.Page = page.GetValueOrDefault();
            this.PageSize = pageSize.GetValueOrDefault();
        }

        public PagedCollection(IEnumerable<T> collection):this(collection,0,0)
        {
         
        }

        public PagedCollection()
        {
            
        }
        
        public int TotalRecords { get; set; }
        
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int PageCount
        {
            get{

                if (PageSize != 0)
                    return (TotalRecords + PageSize - 1) / PageSize;

                return 0;
            }
        }

        public bool HasNext()
        {
            if (TotalRecords <= 0)
                return false;
            //Page +1 because of the indice will be 0
            var actualPage = Page + 1 * PageSize;

            return TotalRecords > actualPage;
        }

        public bool HasPrevious()
        {
            if (TotalRecords <= 0)
                return false;

            return Page > 1;
        }

      
    }
}