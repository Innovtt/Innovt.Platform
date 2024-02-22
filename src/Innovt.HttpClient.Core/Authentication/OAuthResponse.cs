using System.Text.Json.Serialization;

namespace Innovt.HttpClient.Core.Authentication;

/// <summary>
///     Represents the response from an OAuth authentication request.
/// </summary>
public class OAuthResponse
{
    /// <summary>
    ///     Gets or sets the access token obtained from the authentication.
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    /// <summary>
    ///     Gets or sets the token type.
    /// </summary>
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    /// <summary>
    ///     Gets or sets the duration in seconds for which the access token is valid.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    /// <summary>
    ///     Gets or sets the scope of the access token.
    /// </summary>
    [JsonPropertyName("scope")]
    public string Scope { get; set; }
}