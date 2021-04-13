// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;

namespace Innovt.Core.Collections
{
    public interface IPagedCollection<T>
    {
        IEnumerable<T> Items { get; set; }

        string Page { get; set; }

        int PageCount { get; }
        int PageSize { get; set; }
        int TotalRecords { get; set; }
        bool HasNext();
        bool HasPrevious();
    }
}