// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Queries;
using Innovt.Core.Utilities;

namespace Innovt.Contrib.Authorization.Platform.Domain.Filters;

/// <summary>
///     Represents a filter for retrieving roles based on user information.
/// </summary>
public class RoleByUserFilter : IFilter
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RoleByUserFilter" /> class.
    /// </summary>
    /// <param name="externalId">The external ID of the user.</param>
    /// <param name="domainId">The domain ID of the user.</param>
    public RoleByUserFilter(string externalId, string domainId)
    {
        ExternalId = externalId;
        DomainId = domainId;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="RoleByUserFilter" /> class.
    /// </summary>
    public RoleByUserFilter()
    {
    }

    /// <summary>
    ///     Gets or sets the external ID of the user.
    /// </summary>
    public string ExternalId { get; set; }

    /// <summary>
    ///     Gets or sets the domain ID of the user.
    /// </summary>
    public string DomainId { get; set; }

    /// <summary>
    ///     Validates the filter properties.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A collection of validation results.</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ExternalId.IsNullOrEmpty() && DomainId.IsNullOrEmpty())
            yield return new ValidationResult("Domain id or External is required.",
                new[] { nameof(ExternalId), nameof(DomainId) });
    }
}