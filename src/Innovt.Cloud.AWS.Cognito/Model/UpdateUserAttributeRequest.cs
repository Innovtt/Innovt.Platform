
using Innovt.Core.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;

namespace Innovt.Cloud.AWS.Cognito.Model
{
    public class UpdateUserAttributeRequest : RequestBase
    {
        [Required]
        public string AccessToken { get; set; }

        public Dictionary<string,string> Attributes { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AccessToken.IsNullOrEmpty())
            {
                yield return new ValidationResult(Messages.AccessTokenIsRequired, new[] { nameof(AccessToken) });
            }

            if (Attributes.IsNullOrEmpty())
            {
                yield return new ValidationResult(Messages.AttributesIsRequired, new[] { nameof(Attributes) });
            }
        } 
    }
}
