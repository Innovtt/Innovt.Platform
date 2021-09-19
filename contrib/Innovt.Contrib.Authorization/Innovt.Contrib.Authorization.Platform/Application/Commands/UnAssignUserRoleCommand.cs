// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-17

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands
{
    public class UnAssignUserRoleCommand : ICommand
    {
        [Required] public string UserId { get; set; }

        [Required] public string RoleName { get; set; }

        [Required] public string Scope { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}