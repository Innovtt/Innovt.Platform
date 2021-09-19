// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-06-02

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Queries;

namespace Innovt.Contrib.Authorization.Platform.Domain.Filters
{
    public class GroupFilter : IFilter
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}