// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
/// Represents a request for refreshing an authentication token using a refresh token.
/// </summary>
public class RefreshTokenRequest : RequestBase
{
    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    [Required] public string RefreshToken { get; set; }

    /// <summary>
    /// Validates the request data.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>An enumerable collection of validation results.</returns>
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (RefreshToken.IsNullOrEmpty())
            yield return new ValidationResult(Messages.RefreshTokenIsRequired, new[] { nameof(RefreshToken) });
    }
}