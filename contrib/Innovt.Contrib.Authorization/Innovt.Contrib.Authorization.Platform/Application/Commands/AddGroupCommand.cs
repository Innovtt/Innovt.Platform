using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands
{
    public class AddGroupCommand : ICommand
    {
        [Required]
        public string Name { get; set; }
        

        [Required]
        public string Description { get; set; }

        [Required]
        public string Scope { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }

        public AddGroupCommand()
        {
            
        }


        public AddGroupCommand(string name,string scope,string description)
        {
            Name = name;            
            Description = description;
            Scope = scope;
        }
    }
}
