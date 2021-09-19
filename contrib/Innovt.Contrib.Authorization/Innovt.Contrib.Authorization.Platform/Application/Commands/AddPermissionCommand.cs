// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-06-02

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands
{
    public class AddPermissionCommand : ICommand
    {
        public AddPermissionCommand()
        {
        }

        public AddPermissionCommand(string name, string scope, string resource)
        {
            Name = name;
            Scope = scope;
            Resource = resource;
        }

        [Required] public string Name { get; set; }

        [Required] public string Scope { get; set; }

        [Required] public string Resource { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}