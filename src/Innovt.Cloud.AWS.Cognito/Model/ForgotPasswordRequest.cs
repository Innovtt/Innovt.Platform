using Innovt.Core.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;

namespace Innovt.Cloud.AWS.Cognito.Model
{
    public class ForgotPasswordRequest : RequestBase
    {
        [Required] public virtual string UserName { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserName.IsNullOrEmpty())
            {
                yield return new ValidationResult(Messages.UserNameIsRequired, new[] {nameof(UserName)});
            }
        }
    }
}