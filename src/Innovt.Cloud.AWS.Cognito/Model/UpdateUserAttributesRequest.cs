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
public class UpdateUserAttributesRequest : RequestBase
{
    /// <summary>
    ///     Gets or sets the access token associated with the user.
    /// </summary>
    [Required]
    public string AccessToken { get; set; }

    /// <summary>
    ///     Gets or sets the attributes to update for the user.
    /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
    public Dictionary<string, string> Attributes { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

    /// <inheritdoc />
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (AccessToken.IsNullOrEmpty())
            yield return new ValidationResult(Messages.AccessTokenIsRequired, new[] { nameof(AccessToken) });

        if (Attributes.IsNullOrEmpty())
            yield return new ValidationResult(Messages.AttributesIsRequired, new[] { nameof(Attributes) });
    }
}