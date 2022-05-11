// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-20

using Innovt.Core.Collections;
using Innovt.Core.Cqrs.Commands;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands;

public class UnAssignUserRoleCommand : ICommand
{
    [Required] public string UserId { get; set; }
    public IList<RemoveRoleCommand> Roles { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Roles.IsNullOrEmpty())
        {
            yield return new ValidationResult("Roles are required.");
        }
        else
        {
            var errors = Roles.SelectMany(r => r.Validate(validationContext));

            foreach (var error in errors) yield return error;
        }
    }
}