// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Authorization.Platform
// Solution: Innovt.Platform
// Date: 2021-05-20
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Core.Cqrs.Queries;

namespace Innovt.Contrib.Authorization.Platform.Domain.Filters
{
    public class UserByIdFilter : IFilter
    {
        public string Id { get; set; }
        
        public UserByIdFilter(string id)
        {
            Id = id ?? throw new System.ArgumentNullException(nameof(id));            
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}