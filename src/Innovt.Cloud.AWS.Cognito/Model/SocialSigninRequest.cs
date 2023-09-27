// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
/// Represents a request for social sign-in using an OAuth code and redirect URI.
/// </summary>
public class SocialSignInRequest : RequestBase
{
    /// <summary>
    /// Gets or sets the OAuth code obtained from the social authentication provider.
    /// </summary>
    [Required] public virtual string Code { get; set; }

    /// <summary>
    /// Gets or sets the redirect URI to which the user is redirected after social authentication.
    /// </summary>
    [Required] public virtual string RedirectUri { get; set; }

    /// <inheritdoc/>
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Code.IsNullOrEmpty())
            yield return new ValidationResult(Messages.OAuthCodeNotFound, new[] { nameof(Code) });

        if (RedirectUri.IsNullOrEmpty())
            yield return new ValidationResult(Messages.RedirectUriRequired, new[] { nameof(RedirectUri) });
    }
}