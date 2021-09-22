// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-20

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Queries;
using Innovt.Core.Utilities;

namespace Innovt.Contrib.Authorization.Platform.Domain.Filters
{
    public class RoleByUserFilter : IFilter
    {
        public string ExternalId { get; set; }
        public string DomainId { get; set; }

        public RoleByUserFilter(string externalId, string domainId)
        {
            this.ExternalId = externalId;
            this.DomainId = domainId;
        }

        public RoleByUserFilter()
        {
        }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ExternalId.IsNullOrEmpty() && DomainId.IsNullOrEmpty())
                yield return new ValidationResult("Domain id or External is required.",new []{ nameof(ExternalId),nameof(DomainId)});
        }
    }
}