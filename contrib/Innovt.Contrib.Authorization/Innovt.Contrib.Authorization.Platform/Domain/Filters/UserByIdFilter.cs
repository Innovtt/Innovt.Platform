// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Queries;

namespace Innovt.Contrib.Authorization.Platform.Domain.Filters;
/// <summary>
/// Represents a filter for retrieving a user by their ID.
/// </summary>
public class UserByIdFilter : IFilter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserByIdFilter"/> class.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is null.</exception>
    public UserByIdFilter(string id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }
    /// <summary>
    /// Gets or sets the ID of the user.
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Validates the filter properties.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A collection of validation results.</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return new List<ValidationResult>();
    }
}