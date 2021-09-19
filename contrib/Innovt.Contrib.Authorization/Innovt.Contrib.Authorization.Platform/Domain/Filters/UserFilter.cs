// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-08-09

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Queries;

namespace Innovt.Contrib.Authorization.Platform.Domain.Filters
{
    public class UserFilter : IFilter
    {
        public UserFilter(string email)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }

        public UserFilter()
        {
        }

        public string Email { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}