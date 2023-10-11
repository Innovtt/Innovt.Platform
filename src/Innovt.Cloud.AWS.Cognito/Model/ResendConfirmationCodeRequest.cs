// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Collections;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
/// Represents a request for resending a confirmation code to a user.
/// </summary>
public class ResendConfirmationCodeRequest : RequestBase
{
    /// <summary>
    /// Gets or sets the user's username for whom the confirmation code will be resent.
    /// </summary>
    [Required]
    public virtual string UserName { get; set; }

    /// <summary>
    /// Validates the request object.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A collection of validation results.</returns>
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (UserName.IsNullOrEmpty())
            yield return new ValidationResult(Messages.UserNameIsRequired, new[] { nameof(UserName) });
    }
}