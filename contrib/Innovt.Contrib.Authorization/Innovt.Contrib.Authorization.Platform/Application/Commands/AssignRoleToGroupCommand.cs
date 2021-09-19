// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-06-02

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands
{
    public class AssignRoleToGroupCommand : ICommand
    {
        [Required] public Guid RoleId { get; set; }

        [Required] public Guid GroupId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}