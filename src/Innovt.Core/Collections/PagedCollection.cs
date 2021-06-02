// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;
using Innovt.Core.Utilities;

namespace Innovt.Core.Collections
{
    public class PagedCollection<T> : IPagedCollection<T>
    {
        public PagedCollection(IEnumerable<T> collection, int? page = null, int? pageSize = null) : this(collection,
            page?.ToString(), pageSize)
        {
        }


        public PagedCollection(IEnumerable<T> collection, string page = null, int? pageSize = null)
        {
            Items = collection;
            Page = page;
            PageSize = pageSize.GetValueOrDefault();
        }

        public PagedCollection(IEnumerable<T> collection) : this(collection, "0", 0)
        {
        }

        public PagedCollection()
        {
            Items = new List<T>();
        }


        public bool IsNumberPagination => Page.IsNumber();
        public IEnumerable<T> Items { get; set; }

        public int TotalRecords { get; set; }

        public string Page { get; set; }

        public int PageSize { get; set; }

        public int PageCount => PageSize <= 0 ? 0 : TotalRecords / PageSize;

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