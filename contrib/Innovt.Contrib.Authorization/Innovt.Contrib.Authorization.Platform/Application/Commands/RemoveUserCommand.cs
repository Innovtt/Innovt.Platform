// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands;

/// <summary>
/// Represents a command for removing a user.
/// </summary>
public class RemoveUserCommand : ICommand
{
    /// <summary>
    /// Gets or sets the ID of the user to be removed.
    /// </summary>
    [Required]
    public string Id { get; set; }

    /// <summary>
    /// Validates the command properties.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A collection of validation results.</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return new List<ValidationResult>();
    }
}