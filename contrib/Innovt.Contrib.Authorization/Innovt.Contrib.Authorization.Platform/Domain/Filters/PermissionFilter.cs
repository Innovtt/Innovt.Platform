// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-06-02

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Queries;

namespace Innovt.Contrib.Authorization.Platform.Domain.Filters
{
    public class PermissionFilter : IFilter
    {
        public string Scope { get; set; }

        public string Resource { get; set; }

        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Scope == null) yield return new ValidationResult("Scope is required.");
        }
    }
}