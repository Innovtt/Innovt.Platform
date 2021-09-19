// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-06-02

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands
{
    public class AddGroupCommand : ICommand
    {
        public AddGroupCommand()
        {
        }


        public AddGroupCommand(string name, string description)
        {
            Name = name;
            Description = description;
        }

        [Required] public string Name { get; set; }


        [Required] public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}