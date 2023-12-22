// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Innovt.Core.Collections;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands;

/// <summary>
/// Represents a command for adding a user.
/// </summary>
public class AddUserCommand : ICommand
{
    /// <summary>
    /// Gets or sets the ID of the user.
    /// </summary>
    [Required]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the domain ID of the user.
    /// </summary>
    [Required]
    public string DomainId { get; set; }

    /// <summary>
    /// Gets or sets the list of roles associated with the user.
    /// </summary>
    public IList<AddRoleCommand> Roles { get; set; }

    /// <summary>
    /// Validates the command properties, including roles.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A collection of validation results.</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Id.IsNullOrEmpty()) yield return new ValidationResult("Id is required.");

        if (DomainId.IsNullOrEmpty()) yield return new ValidationResult("DomainId is required.");

        if (Roles.IsNotNullOrEmpty())
        {
            var errors = Roles.SelectMany(r => r.Validate(validationContext));

            foreach (var error in errors) yield return error;
        }
    }
}