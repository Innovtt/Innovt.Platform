// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
///     Represents a response containing refreshed authentication tokens.
/// </summary>
public class RefreshTokenResponse
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
    ///     Gets or sets the expiration time of the access token in seconds.
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    ///     Gets or sets the type of token (e.g., "Bearer").
    /// </summary>
    public string TokenType { get; set; }
}