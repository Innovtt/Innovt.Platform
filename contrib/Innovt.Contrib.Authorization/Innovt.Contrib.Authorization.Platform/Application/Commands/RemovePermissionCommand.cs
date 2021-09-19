// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-06-02

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Attributes;
using Innovt.Core.Cqrs.Commands;

namespace Innovt.Contrib.Authorization.Platform.Application.Commands
{
    public class RemovePermissionCommand : ICommand
    {
        [RequiredGuid] public Guid Id { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}