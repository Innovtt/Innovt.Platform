// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands;

/// <summary>
///     Represents a command for assigning roles to a user.
/// </summary>
public class AssignRoleCommand : ICommand
{
    /// <summary>
    ///     Gets or sets the ID of the user to whom roles will be assigned.
    /// </summary>
    [Required]
    public string UserId { get; set; }

    /// <summary>
    ///     Gets or sets the list of roles to be assigned to the user.
    /// </summary>
    public IList<AddRoleCommand> Roles { get; set; }

    /// <summary>
    ///     Validates the command properties, including roles.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A collection of validation results.</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Roles == null)
        {
            yield return new ValidationResult("Role is required.");
        }
        else
        {
            var errors = Roles.SelectMany(r => r.Validate(validationContext));

            foreach (var error in errors) yield return error;
        }
    }
}