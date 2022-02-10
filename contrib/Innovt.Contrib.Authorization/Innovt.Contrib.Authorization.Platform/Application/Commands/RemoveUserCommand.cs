// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-21

using Innovt.Core.Cqrs.Commands;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands
{
    public class RemoveUserCommand : ICommand
    {
        [Required] public string Id { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}