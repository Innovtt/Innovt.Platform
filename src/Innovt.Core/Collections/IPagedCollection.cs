// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Innovt.Core.Collections
{
    [SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "<Pending>")]
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