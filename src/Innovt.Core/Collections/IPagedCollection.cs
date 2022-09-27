// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Innovt.Core.Collections;

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