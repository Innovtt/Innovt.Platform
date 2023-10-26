// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Innovt.Cloud.AWS.Cognito.Resources;
using Innovt.Core.Collections;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
/// Represents a request to update user attributes.
/// </summary>
public class LinkSocialAccountRequest : RequestBase
{   
   /// <summary>
   /// The email of the user to link the account.
   /// </summary>
    [Required]
    public string Email { get; set; }
    
   /// <summary>
   /// The provider name of the user to link the account. Can be Google, Facebook, etc.
   /// </summary>
    [Required]
    public string ProviderName { get; set; }
    
   /// <summary>
   /// The parameter value of the user to link the account. Can be the email, the id, sub, etc.
   /// </summary>
    [Required]
    public string ProviderValue { get; set; }
    
    /// <inheritdoc/>
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Email.IsNullOrEmpty())
            yield return new ValidationResult(Messages.EmailIsRequired, new[] { nameof(Email) });
        
    }
}