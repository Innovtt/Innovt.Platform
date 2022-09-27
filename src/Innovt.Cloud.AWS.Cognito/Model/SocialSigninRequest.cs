// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Cognito.Model;

public class SocialSignInRequest : RequestBase
{
    [Required] public virtual string Code { get; set; }

    [Required] public virtual string RedirectUri { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Code.IsNullOrEmpty())
            yield return new ValidationResult(Messages.OAuthCodeNotFound, new[] { nameof(Code) });

        if (RedirectUri.IsNullOrEmpty())
            yield return new ValidationResult(Messages.RedirectUriRequired, new[] { nameof(RedirectUri) });
    }
}