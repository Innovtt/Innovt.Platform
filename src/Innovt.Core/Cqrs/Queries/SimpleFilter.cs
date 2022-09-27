// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Core.Cqrs.Queries;

public class SimpleFilter<T> : IFilter
{
    public SimpleFilter(T data)
    {
        Data = data;
    }

    public T Data { get; set; }

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return new List<ValidationResult>();
    }
}