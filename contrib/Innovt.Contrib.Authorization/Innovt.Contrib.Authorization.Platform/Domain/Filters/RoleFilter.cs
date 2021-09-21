// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-20

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Queries;

namespace Innovt.Contrib.Authorization.Platform.Domain.Filters
{
    public class RoleFilter : IFilter
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Scope { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Scope == null) yield return new ValidationResult("Scope is required.");
        }
    }
}