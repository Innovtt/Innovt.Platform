// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
/// Represents the response of an authentication challenge, including metadata and authentication result.
/// </summary>
public class AuthChallengeResponse
{
    /// <summary>
    /// Gets or sets the metadata associated with the authentication challenge.
    /// </summary>
    public IDictionary<string, string> Metadata { get; set; }

    /// <summary>
    /// Gets or sets the authentication result of the challenge.
    /// </summary>
    public SignInResponse AuthenticationResult { get; set; }
}