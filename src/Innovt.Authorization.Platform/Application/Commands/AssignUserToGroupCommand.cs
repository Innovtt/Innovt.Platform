using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Authorization.Platform.Application.Commands
{
    public class AssignUserToGroupCommand : ICommand
    {
        public string UserId { get; set; }

        public Guid GroupId { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
