using Innovt.Domain.Core.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SampleAspNetWebApiTest;

public class Wheather : Entity, IValidatableObject
{
    public string Name { get; set; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Name is null)
        {
            yield return new ValidationResult("INvalid name");
        }
    }
}