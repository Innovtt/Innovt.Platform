// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Collections;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
///   This requests represents a request to clear a social account from a user's profile.
/// </summary>
public class ClearSocialAccountRequest : RequestBase
{
    public ClearSocialAccountRequest()
    {
    }

    public ClearSocialAccountRequest(string email)
    {
        Email = email;
    }

    [Required] [EmailAddress] public string Email { get; set; }

    /// <inheritdoc />
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Email.IsNullOrEmpty())
            yield return new ValidationResult(Messages.EmailIsRequired, [nameof(Email)]);
    }
}