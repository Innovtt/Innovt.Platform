// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Queries;
using Innovt.Core.Utilities;

namespace Innovt.Contrib.Authorization.Platform.Domain.Filters;

public class RoleByUserFilter : IFilter
{
    public RoleByUserFilter(string externalId, string domainId)
    {
        ExternalId = externalId;
        DomainId = domainId;
    }

    public RoleByUserFilter()
    {
    }

    public string ExternalId { get; set; }
    public string DomainId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ExternalId.IsNullOrEmpty() && DomainId.IsNullOrEmpty())
            yield return new ValidationResult("Domain id or External is required.",
                new[] { nameof(ExternalId), nameof(DomainId) });
    }
}