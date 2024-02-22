// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Collections;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
///     Represents a request for initiating the forgot password process.
/// </summary>
public class ForgotPasswordRequest : RequestBase
{
    /// <summary>
    ///     Gets or sets the username associated with the user who forgot their password.
    /// </summary>
    [Required]
    public virtual string UserName { get; set; }

    /// <summary>
    ///     Validates the <see cref="ForgotPasswordRequest" /> instance.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>An <see cref="IEnumerable{ValidationResult}" /> containing validation errors, if any.</returns>
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (UserName.IsNullOrEmpty())
            yield return new ValidationResult(Messages.UserNameIsRequired, new[] { nameof(UserName) });
    }
}