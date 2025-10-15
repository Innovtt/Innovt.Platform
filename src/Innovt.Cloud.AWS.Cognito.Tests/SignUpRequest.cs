using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Model;
using Innovt.Core.Utilities;

namespace Innovt.Cloud.AWS.Cognito.Tests;

public class SignUpRequest : RequestBase, ISignUpRequest
{
    public string Name { get; set; }
    public string Family_Name { get; set; }

    public string Phone_Number { get; set; }

    public string Profile { get; set; }
    private Guid? InvitedById { get; set; }

    [Required] [EmailAddress] public string UserName { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 8)]
    public string Password { get; set; }

    public Dictionary<string, string> CustomAttributes { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (UserName.IsNullOrEmpty())
            yield return new ValidationResult("error", new[] { nameof(Family_Name) });
    }

    public void SetInvitedById(Guid? invitedById)
    {
        InvitedById = invitedById;
    }

    /// <summary>
    ///     We are working with methods to avoid direct access to the property. The Innovt framework will use reflection to
    ///     access the property.
    /// </summary>
    /// <returns></returns>
    public Guid? GetInviteById()
    {
        return InvitedById;
    }
}