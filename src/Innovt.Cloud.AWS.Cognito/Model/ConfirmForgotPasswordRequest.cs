// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Cognito
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;

namespace Innovt.Cloud.AWS.Cognito.Model
{
    public class ConfirmForgotPasswordRequest : RequestBase
    {
        [Required] public string UserName { get; set; }

        [Required] public string Password { get; set; }
        [Required] public string ConfirmPassword { get; set; }
        [Required] public string ConfirmationCode { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password != ConfirmPassword)
                yield return new ValidationResult(Messages.PasswordsDoNotMatch, new[] {nameof(Password)});
        }
    }
}