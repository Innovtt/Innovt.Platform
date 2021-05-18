using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Attributes;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Authorization.Platform.Application.Commands
{
    public class AssignUserToGroupCommand : ICommand
    {
        [Required]
        public string UserId { get; set; }

        [RequiredGuid]
        public Guid GroupId { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
