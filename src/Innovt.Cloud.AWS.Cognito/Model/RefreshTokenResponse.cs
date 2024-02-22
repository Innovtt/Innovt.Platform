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
    [JsonPropertyName("id_token")]
    public string IdToken { get; set; }

    /// <summary>
    ///     Gets or sets the access token.
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    /// <summary>
    ///     Gets or sets the expiration time of the access token in seconds.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    /// <summary>
    ///     Gets or sets the type of token (e.g., "Bearer").
    /// </summary>
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
}