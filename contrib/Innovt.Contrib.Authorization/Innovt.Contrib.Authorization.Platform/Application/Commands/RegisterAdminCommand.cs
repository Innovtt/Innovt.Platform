// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands;

/// <summary>
/// Represents a command for registering an administrator.
/// </summary>
public class RegisterAdminCommand : ICommand
{
    /// <summary>
    /// Gets or sets the email address of the administrator.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the name of the administrator.
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the password for the administrator.
    /// </summary>
    [Required]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the confirmation password for the administrator.
    /// </summary>
    [Required]
    [Compare(nameof(Password), ErrorMessage = "Password doesn't match.")]
    public string ConfirmPassword { get; set; }

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