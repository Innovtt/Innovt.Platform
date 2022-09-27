// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands;

public class RegisterAdminCommand : ICommand
{
    [Required] [EmailAddress] public string Email { get; set; }

    [Required] public string Name { get; set; }

    [Required] public string Password { get; set; }

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Password doesn't match.")]
    public string ConfirmPassword { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return new List<ValidationResult>();
    }
}