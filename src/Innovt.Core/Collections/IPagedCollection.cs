using System.Collections.Generic;

namespace Innovt.Core.Collections
{
    public interface IPagedCollection<T> where T : class
    {
        IEnumerable<T> Items { get; set; }

        int Page { get; set; }
      
        int PageCount { get; }
        int PageSize { get; set; }
        int TotalRecords { get; set; }
        bool HasNext();
        bool HasPrevious();
    }
}