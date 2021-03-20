using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Core.Cqrs.Queries
{
    public class PagedFilterBase : PagedFilterBase<string>
    {
    }

    public class PagedFilterBase<T> : IPagedFilter
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public T Term { get; set; }

        public string OrderBy { get; set; }

        public string OrderByDirection { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}