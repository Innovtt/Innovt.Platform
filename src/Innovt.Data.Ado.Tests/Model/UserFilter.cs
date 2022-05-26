using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Queries;

namespace Innovt.Data.Ado.Tests.Model
{
    public class UserFilter : IPagedFilter
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}