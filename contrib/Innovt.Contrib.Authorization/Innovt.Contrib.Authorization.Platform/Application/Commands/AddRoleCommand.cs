// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Commands;
using Innovt.Core.Utilities;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands;
/// <summary>
/// Represents a command for adding a role.
/// </summary>
public class AddRoleCommand : ICommand
{
    /// <summary>
    /// Gets or sets the role name.
    /// </summary>
    [Required] public string RoleName { get; set; }
    /// <summary>
    /// Gets or sets the scope of the role.
    /// </summary>
    [Required] public string Scope { get; set; }
    /// <summary>
    /// Validates the command properties.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A collection of validation results.</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Scope.IsNullOrEmpty()) yield return new ValidationResult("Scope is required.");

        if (RoleName.IsNullOrEmpty()) yield return new ValidationResult("RoleName is required.");
    }
}