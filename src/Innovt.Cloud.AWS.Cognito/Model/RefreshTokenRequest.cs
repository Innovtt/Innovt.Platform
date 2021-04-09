// Solution: Innovt.Platform
// Project: Innovt.Cloud.AWS.Cognito
// User: Michel Magalhães
// Date: 2020/02/03 at 10:12 PM

using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Cloud.AWS.Cognito.Model
{
    public class RefreshTokenRequest : RequestBase
    {
        [Required] public string RefreshToken { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RefreshToken.IsNullOrEmpty())
                yield return new ValidationResult(Messages.RefreshTokenIsRequired, new[] { nameof(RefreshToken) });
        }
    }
}