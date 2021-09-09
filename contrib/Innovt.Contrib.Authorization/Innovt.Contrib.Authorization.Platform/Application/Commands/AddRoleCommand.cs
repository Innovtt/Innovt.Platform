using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands
{
    public class AddRoleCommand:ICommand
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Scope { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
        public IList<Guid> PermissionIds { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Scope == null)
            {
                yield return new ValidationResult("Scope is required.");
            }
        }

        public AddRoleCommand()
        {
        }


        public AddRoleCommand(string name, string description, IList<Guid> permissionIds)
        {
            Name = name;
            Description = description;
            PermissionIds = permissionIds;
        }
    }
}
