// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.ComponentModel.DataAnnotations;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
/// Represents a base class for sign-in requests.
/// </summary>
public abstract class SignInRequestBase : RequestBase
{
    /// <summary>
    /// Gets or sets the user's username.
    /// </summary>
    [Required]
    public string UserName { get; set; }
}