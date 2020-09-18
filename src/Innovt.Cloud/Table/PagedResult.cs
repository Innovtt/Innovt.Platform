
using System.Collections.Generic;

namespace Innovt.Cloud.Table
{
    public sealed class PagedResult<T>
    {
        public string PaginationToken { get; set; }

        public IList<T> Items { get; set; }

    }
}