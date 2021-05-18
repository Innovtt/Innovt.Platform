using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Authorization.Platform.Application.Commands
{
    public class AddRoleCommand:ICommand
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        
#pragma warning disable CA2227 // Collection properties should be read only
        public IList<Guid> PermissionIds { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
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
