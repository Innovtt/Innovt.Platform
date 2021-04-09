using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Cloud.AWS.Cognito.Model
{
    public class SignOutRequest : RequestBase
    {
        [Required] public virtual string AccessToken { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AccessToken.IsNullOrEmpty())
                yield return new ValidationResult(Messages.AccessTokenIsRequired, new[] { nameof(AccessToken) });
        }
    }
}