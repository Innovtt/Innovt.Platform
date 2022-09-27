// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Collections;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands;

public class RemoveRoleCommand : ICommand
{
    [Required] public string RoleName { get; set; }

    [Required] public string Scope { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Scope.IsNullOrEmpty()) yield return new ValidationResult("Scope is required.");

        if (RoleName.IsNullOrEmpty()) yield return new ValidationResult("RoleName is required.");
    }
}