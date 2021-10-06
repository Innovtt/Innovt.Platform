﻿// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-20

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands
{
    public class AssignRoleCommand : ICommand
    {
        [Required] public string UserId { get; set; }

        [Required] public string RoleName { get; set; }

        [Required] public string Scope { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Scope == null) yield return new ValidationResult("Scope is required.");
        }
    }
}