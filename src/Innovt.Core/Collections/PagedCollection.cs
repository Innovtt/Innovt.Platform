using Innovt.Core.Utilities;
using System.Collections.Generic;

namespace Innovt.Core.Collections
{
    public class PagedCollection<T> : IPagedCollection<T>
    {
        public IEnumerable<T> Items { get; set; }


        public PagedCollection(IEnumerable<T> collection, int? page = null, int? pageSize = null):this(collection,pageSize?.ToString(),pageSize)
        {  
        }


        public PagedCollection(IEnumerable<T> collection,string page=null,int? pageSize=null)
        {
            this.Items = collection;
            this.Page = page;
            this.PageSize = pageSize.GetValueOrDefault();
        }

        public PagedCollection(IEnumerable<T> collection):this(collection,"0",0)
        {
         
        }

        public PagedCollection()
        {
            this.Items = new List<T>();
        }
        
        public int TotalRecords { get; set; }
        
        public string Page { get; set; }

        public int PageSize { get; set; }


        public bool IsNumberPagination
        {
            get
            {
                return this.Page.IsNumber();
            }
        }

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
            if (TotalRecords <= 0 || !IsNumberPagination)
                return false;

            //Page +1 because of the indice will be 0
            var actualPage = int.Parse(Page) + 1 * PageSize;

            return TotalRecords > actualPage;
        }

        public bool HasPrevious()
        {
            if (TotalRecords <= 0 || !IsNumberPagination)
                return false;

            return int.Parse(Page) > 1;
        }

      
    }
}