// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
///     Represents a request to change a user's password, including required parameters and validation.
/// </summary>
public class ChangePasswordRequest : RequestBase
{
    /// <summary>
    ///     Gets or sets the access token associated with the user.
    /// </summary>
    [Required]
    public string AccessToken { get; set; }

    /// <summary>
    ///     Gets or sets the user's previous password.
    /// </summary>
    [Required]
    [StringLength(20, MinimumLength = 4)]
    public string PreviousPassword { get; set; }

    /// <summary>
    ///     Gets or sets the proposed new password.
    /// </summary>
    [Required]
    [StringLength(18, MinimumLength = 4)]
    public string ProposedPassword { get; set; }

    /// <summary>
    ///     Gets or sets the confirmation of the proposed new password.
    /// </summary>
    [Required]
    [StringLength(18, MinimumLength = 4)]
    public string ConfirmProposedPassword { get; set; }

    /// <summary>
    ///     Validates the properties of the <see cref="ChangePasswordRequest" /> object.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>An <see cref="IEnumerable{ValidationResult}" /> containing validation results.</returns>
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (AccessToken.IsNullOrEmpty())
            yield return new ValidationResult(Messages.EmailIsRequired, new[] { nameof(AccessToken) });

        if (ConfirmProposedPassword.IsNullOrEmpty())
            yield return new ValidationResult(Messages.ConfirmPasswordIsRequired,
                new[] { nameof(ConfirmProposedPassword) });

        if (PreviousPassword.IsNullOrEmpty())
            yield return new ValidationResult(Messages.CurrentPasswordRequired, new[] { nameof(PreviousPassword) });

        if (ProposedPassword != ConfirmProposedPassword)
            yield return new ValidationResult(Messages.PasswordsDoNotMatch,
                new[] { nameof(ConfirmProposedPassword), nameof(ProposedPassword) });
    }
}