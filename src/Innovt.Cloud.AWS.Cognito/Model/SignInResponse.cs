// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
///     Represents a response containing sign-in information.
/// </summary>
public class SignInResponse
{
    /// <summary>
    ///     Gets or sets the ID token.
    /// </summary>
    public string IdToken { get; set; }

    /// <summary>
    ///     Gets or sets the access token.
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    ///     Gets or sets the expiration time in seconds for the access token.
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    ///     Gets or sets the refresh token.
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    ///     Gets or sets the token type.
    /// </summary>
    public string TokenType { get; set; }

    /// <summary>
    ///     Gets or sets the sign-in type.
    /// </summary>
    public string SignInType { get; set; }

    /// <summary>
    ///     Gets or sets the session.
    /// </summary>
    public string Session { get; set; }

    /// <summary>
    ///     Gets or sets the challenge name.
    /// </summary>
    public string ChallengeName { get; set; }

    /// <summary>
    ///     Gets or sets the challenge parameters.
    /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
    public Dictionary<string, string> ChallengeParameters { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
}