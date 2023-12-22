// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
///     Represents the interface for sign-up requests with user information and custom attributes.
/// </summary>
public interface ISignUpRequest : IRequestBase
{
    /// <summary>
    ///     Gets or sets the username associated with the sign-up request.
    /// </summary>
    string UserName { get; set; }

    /// <summary>
    ///     Gets or sets the password associated with the sign-up request.
    /// </summary>
    string Password { get; set; }

    /// <summary>
    ///     Gets or sets the custom attributes associated with the sign-up request.
    /// </summary>
    public Dictionary<string, string> CustomAttributes { get; set; }
}