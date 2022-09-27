﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Innovt.Core.Collections;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands;

public class AddUserCommand : ICommand
{
    [Required] public string Id { get; set; }

    [Required] public string DomainId { get; set; }

    public IList<AddRoleCommand> Roles { get; set; }

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