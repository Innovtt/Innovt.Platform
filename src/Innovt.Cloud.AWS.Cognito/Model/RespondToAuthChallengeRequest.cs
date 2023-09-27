// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
/// Represents a request for responding to an authentication challenge.
/// </summary>
public class RespondToAuthChallengeRequest : RequestBase
{
    /// <summary>
    /// Gets or sets the user's username.
    /// </summary>
    [Required] public virtual string UserName { get; set; }

    /// <summary>
    /// Gets or sets the session token associated with the authentication challenge.
    /// </summary>
    [Required] public virtual string Session { get; set; }

    /// <summary>
    /// Gets or sets the name of the challenge.
    /// </summary>
    [Required] public virtual string ChallengeName { get; set; }

    /// <summary>
    /// Gets or sets the confirmation code used to respond to the challenge.
    /// </summary>
    public virtual string ConfirmationCode { get; set; }

    /// <summary>
    /// Gets or sets the user's password used to respond to the challenge.
    /// </summary>
    public virtual string Password { get; set; }

    /// <summary>
    /// Validates the request object.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A collection of validation results.</returns>
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (UserName.IsNullOrEmpty())
            yield return new ValidationResult(Messages.UserNameIsRequired, new[] { nameof(UserName) });

        if (ChallengeName.IsNullOrEmpty())
            yield return new ValidationResult(Messages.ChallengeNameIsRequired, new[] { nameof(ChallengeName) });

        if (Session.IsNullOrEmpty())
            yield return new ValidationResult(Messages.InvalidChallengeSession, new[] { nameof(Session) });
    }
}