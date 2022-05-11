// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Domain.Core.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Domain.Contacts;

public class Contact : ValueObject<int>, IValidatableObject
{
    [Required] public string Name { get; set; }

    /// <summary>
    ///     For example Home, Office etc
    /// </summary>
    [Required]
    public string Description { get; set; }

    [Required] public ContactType Type { get; set; }

    [Required] public string Value { get; set; }

    public bool IsDeleted { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Type == null)
        {
            yield return new ValidationResult("Contact type is required");
        }
        else
        {
            if (Type.Validate(Value))
                yield return new ValidationResult($"The value {Value} is not valid for {Type.Name}.");
        }
    }
}