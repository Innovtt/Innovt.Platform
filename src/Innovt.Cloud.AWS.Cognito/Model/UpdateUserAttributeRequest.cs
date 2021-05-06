// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Cognito
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Collections;

namespace Innovt.Cloud.AWS.Cognito.Model
{
    public class UpdateUserAttributeRequest : RequestBase
    {
        [Required] public string AccessToken { get; set; }

        public Dictionary<string, string> Attributes { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AccessToken.IsNullOrEmpty())
                yield return new ValidationResult(Messages.AccessTokenIsRequired, new[] {nameof(AccessToken)});

            if (Attributes.IsNullOrEmpty())
                yield return new ValidationResult(Messages.AttributesIsRequired, new[] {nameof(Attributes)});
        }
    }
}