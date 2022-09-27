// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Cognito.Model;

public class RefreshTokenRequest : RequestBase
{
    [Required] public string RefreshToken { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (RefreshToken.IsNullOrEmpty())
            yield return new ValidationResult(Messages.RefreshTokenIsRequired, new[] { nameof(RefreshToken) });
    }
}