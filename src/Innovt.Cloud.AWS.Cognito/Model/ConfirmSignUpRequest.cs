// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Collections;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
///     Represents a request to confirm a user sign-up.
/// </summary>
public class ConfirmSignUpRequest : RequestBase
{
    /// <summary>
    ///     Gets or sets the username of the user to confirm.
    /// </summary>
    [Required]
    public virtual string UserName { get; set; }

    /// <summary>
    ///     Gets or sets the confirmation code associated with the user's sign-up request.
    /// </summary>
    [Required]
    [StringLength(20, MinimumLength = 4)]
    public virtual string ConfirmationCode { get; set; }

    /// <summary>
    ///     Validates the properties of the <see cref="ConfirmSignUpRequest" /> object.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>An <see cref="IEnumerable{ValidationResult}" /> containing validation results.</returns>
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (UserName.IsNullOrEmpty())
            yield return new ValidationResult(Messages.UserNameIsRequired, new[] { nameof(UserName) });

        if (ConfirmationCode.IsNullOrEmpty())
            yield return new ValidationResult(Messages.ConfirmationCodeRequired, new[] { nameof(ConfirmationCode) });
    }
}