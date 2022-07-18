// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Cognito
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Cloud.AWS.Cognito.Model;

public class SignInRequest : SignInRequestBase
{
    internal const string ValidateContextCreate = "Create";

    [Required]
    [StringLength(30, MinimumLength = 4)]
    public virtual string Password { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (UserName.IsNullOrEmpty())
            yield return new ValidationResult(Messages.EmailIsRequired, new[] { nameof(UserName) });

        if (!ValidateContextCreate.Equals(validationContext?.ObjectInstance?.ToString(),
                StringComparison.Ordinal)) yield break;


        if (IpAddress.IsNullOrEmpty())
            yield return new ValidationResult(Messages.IpAddressRequired, new[] { nameof(IpAddress) });

        if (ServerPath.IsNullOrEmpty())
            yield return new ValidationResult(Messages.ServerPathRequired, new[] { nameof(ServerPath) });
    }
}