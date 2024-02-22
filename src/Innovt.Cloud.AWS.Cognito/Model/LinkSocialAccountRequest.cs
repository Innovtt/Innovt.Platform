// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Collections;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
///     Represents a request to update user attributes.
/// </summary>
public class LinkSocialAccountRequest : RequestBase
{
    public LinkSocialAccountRequest()
    {
    }

    public LinkSocialAccountRequest(string email, string userName)
    {
        Email = email;
        UserName = userName;
    }

    /// <summary>
    ///     The email of the user to link the account.
    /// </summary>
    [Required]
    public string Email { get; set; }

    [Required] public string UserName { get; set; }

    /// <inheritdoc />
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Email.IsNullOrEmpty())
            yield return new ValidationResult(Messages.EmailIsRequired, new[] { nameof(Email) });
    }
}