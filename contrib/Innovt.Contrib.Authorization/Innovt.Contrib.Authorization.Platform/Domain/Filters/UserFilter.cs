// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Queries;

namespace Innovt.Contrib.Authorization.Platform.Domain.Filters;

/// <summary>
/// Represents a filter for retrieving a user by their email.
/// </summary>
public class UserFilter : IFilter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserFilter"/> class.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="email"/> is null.</exception>
    public UserFilter(string email)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserFilter"/> class.
    /// </summary>
    public UserFilter()
    {
    }

    /// <summary>
    /// Gets or sets the email of the user.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Validates the filter properties.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A collection of validation results.</returns>
    /// <remarks>This method is not implemented and will throw <see cref="NotImplementedException"/>.</remarks>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new NotImplementedException();
    }
}