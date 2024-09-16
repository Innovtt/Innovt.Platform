
namespace Innovt.HttpClient.Core.Authentication;

/// <summary>
///     Represents the response from an OAuth authentication request.
/// </summary>
public class OAuthResponse
{
    /// <summary>
    ///     Gets or sets the access token obtained from the authentication.
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    ///     Gets or sets the token type.
    /// </summary>
    public string TokenType { get; set; }

    /// <summary>
    ///     Gets or sets the duration in seconds for which the access token is valid.
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    ///     Gets or sets the scope of the access token.
    /// </summary>
    public string Scope { get; set; }
}