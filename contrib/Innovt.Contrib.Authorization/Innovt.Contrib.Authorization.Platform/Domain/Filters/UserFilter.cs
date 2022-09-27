// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Queries;

namespace Innovt.Contrib.Authorization.Platform.Domain.Filters;

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