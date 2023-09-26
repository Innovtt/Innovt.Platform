// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
/// Represents a request to sign out with an access token.
/// </summary>
public class SignOutRequest : RequestBase
{
    /// <summary>
    /// Gets or sets the access token used for signing out.
    /// </summary>
    [Required] public virtual string AccessToken { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (AccessToken.IsNullOrEmpty())
            yield return new ValidationResult(Messages.AccessTokenIsRequired, new[] { nameof(AccessToken) });
    }
}