// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
///     Represents a request to confirm a forgotten password, including a password reset operation.
/// </summary>
public class ConfirmForgotPasswordRequest : RequestBase
{
    /// <summary>
    ///     Gets or sets the user's username.
    /// </summary>
    [Required]
    public string UserName { get; set; }

    /// <summary>
    ///     Gets or sets the new password for the user.
    /// </summary>
    [Required]
    public string Password { get; set; }

    /// <summary>
    ///     Gets or sets the confirmation of the new password.
    /// </summary>
    [Required]
    public string ConfirmPassword { get; set; }

    /// <summary>
    ///     Gets or sets the confirmation code used for password reset.
    /// </summary>
    [Required]
    public string ConfirmationCode { get; set; }

    /// <summary>
    ///     Validates the properties of the <see cref="ConfirmForgotPasswordRequest" /> object.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>An <see cref="IEnumerable{ValidationResult}" /> containing validation results.</returns>
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Password != ConfirmPassword)
            yield return new ValidationResult(Messages.PasswordsDoNotMatch, new[] { nameof(Password) });
    }
}