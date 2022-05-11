// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Cognito
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Cloud.AWS.Cognito.Model;

public class OtpSignInRequest : SignInRequestBase
{
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (UserName.IsNullOrEmpty())
            yield return new ValidationResult(Messages.UserNameIsRequired, new[] { nameof(UserName) });
    }
}