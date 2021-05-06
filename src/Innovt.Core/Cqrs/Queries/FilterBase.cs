// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Core.Cqrs.Queries
{
    public class PagedFilterBase : PagedFilterBase<string>
    {
    }

    public class PagedFilterBase<T> : IPagedFilter
    {
        public T Term { get; set; }

        public string OrderBy { get; set; }

        public string OrderByDirection { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}