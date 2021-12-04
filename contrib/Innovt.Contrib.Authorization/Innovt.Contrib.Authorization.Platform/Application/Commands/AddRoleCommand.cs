// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-20

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Commands;
using Innovt.Core.Utilities;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands
{
    public class AddRoleCommand : ICommand
    {
        [Required] 
        public string RoleName { get; set; }

        [Required] public string Scope { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Scope.IsNullOrEmpty())
            {
                yield return new ValidationResult("Scope is required.");
            }

            if (RoleName.IsNullOrEmpty())
            {
                yield return new ValidationResult("RoleName is required.");
            }
        }
    }
}