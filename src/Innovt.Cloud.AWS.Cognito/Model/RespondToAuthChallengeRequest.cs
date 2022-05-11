// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Cloud.AWS.Cognito
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Cloud.AWS.Cognito.Model;

public class RespondToAuthChallengeRequest : RequestBase
{
    [Required] public virtual string UserName { get; set; }
    [Required] public virtual string Session { get; set; }
    [Required] public virtual string ChallengeName { get; set; }

    public virtual string ConfirmationCode { get; set; }

    public virtual string Password { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (UserName.IsNullOrEmpty())
            yield return new ValidationResult(Messages.UserNameIsRequired, new[] { nameof(UserName) });

        if (ChallengeName.IsNullOrEmpty())
            yield return new ValidationResult(Messages.ChallengeNameIsRequired, new[] { nameof(ChallengeName) });

        if (Session.IsNullOrEmpty())
            yield return new ValidationResult(Messages.InvalidChallengeSession, new[] { nameof(Session) });
    }
}