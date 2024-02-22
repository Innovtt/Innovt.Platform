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
public class DeleteUserAccountRequest : RequestBase
{
    public DeleteUserAccountRequest()
    {
    }

    public DeleteUserAccountRequest(string userName)
    {
        UserName = userName;
    }

    [Required] public string UserName { get; set; }

    /// <inheritdoc />
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (UserName.IsNullOrEmpty())
            yield return new ValidationResult(Messages.EmailIsRequired, new[] { nameof(UserName) });
    }
}