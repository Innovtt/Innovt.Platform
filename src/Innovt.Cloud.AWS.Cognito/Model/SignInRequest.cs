// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
/// Represents a request for user sign-in.
/// </summary>
public class SignInRequest : SignInRequestBase
{
    internal const string ValidateContextCreate = "Create";

    /// <summary>
    /// Gets or sets the user's password.
    /// </summary>
    [Required]
    [StringLength(30, MinimumLength = 4)]
    public virtual string Password { get; set; }

    /// <summary>
    /// Validates the request object.
    /// </summary>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A collection of validation results.</returns>
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (UserName.IsNullOrEmpty())
            yield return new ValidationResult(Messages.EmailIsRequired, new[] { nameof(UserName) });

        if (!ValidateContextCreate.Equals(validationContext?.ObjectInstance?.ToString(),
                StringComparison.Ordinal)) yield break;


        if (IpAddress.IsNullOrEmpty())
            yield return new ValidationResult(Messages.IpAddressRequired, new[] { nameof(IpAddress) });

        if (ServerPath.IsNullOrEmpty())
            yield return new ValidationResult(Messages.ServerPathRequired, new[] { nameof(ServerPath) });
    }
}