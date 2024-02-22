// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
///     Represents a response containing sign-in information.
/// </summary>
public class SignInResponse
{
    /// <summary>
    ///     Gets or sets the ID token.
    /// </summary>
    [JsonPropertyName("id_token")]
    public string IdToken { get; set; }

    /// <summary>
    ///     Gets or sets the access token.
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    /// <summary>
    ///     Gets or sets the expiration time in seconds for the access token.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    /// <summary>
    ///     Gets or sets the refresh token.
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    /// <summary>
    ///     Gets or sets the token type.
    /// </summary>
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    /// <summary>
    ///     Gets or sets the sign-in type.
    /// </summary>
    [JsonPropertyName("signInType")]
    public string SignInType { get; set; }

    /// <summary>
    ///     Gets or sets the session.
    /// </summary>
    [JsonPropertyName("session")]
    public string Session { get; set; }

    /// <summary>
    ///     Gets or sets the challenge name.
    /// </summary>
    [JsonPropertyName("ChallengeName")]
    public string ChallengeName { get; set; }

    /// <summary>
    ///     Gets or sets the challenge parameters.
    /// </summary>
    [JsonPropertyName("ChallengeParameters")]
#pragma warning disable CA2227 // Collection properties should be read only
    public Dictionary<string, string> ChallengeParameters { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
}