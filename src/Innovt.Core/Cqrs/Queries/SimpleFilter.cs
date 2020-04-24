using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Core.Cqrs.Queries
{
    public class SimpleFilter<T> : IFilter
    {
        public T Data { get; set; }

        public SimpleFilter(T data)
        {
            Data = data;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}