using System;

namespace Innovt.HttpClient.Core;

/// <summary>
/// Represents the context for interacting with an API.
/// </summary>
public sealed class ApiContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiContext"/> class.
    /// </summary>
    /// <param name="environment">The environment associated with the API context.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="environment"/> is null.</exception>
    public ApiContext(IEnvironment environment)
    {
        Environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiContext"/> class with an access token.
    /// </summary>
    /// <param name="environment">The environment associated with the API context.</param>
    /// <param name="accessToken">The access token.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="environment"/> or <paramref name="accessToken"/> is null.</exception>
    public ApiContext(IEnvironment environment, string accessToken) : this(environment)
    {
        AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiContext"/> class with basic credentials.
    /// </summary>
    /// <param name="environment">The environment associated with the API context.</param>
    /// <param name="credential">The basic credential.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="environment"/> or <paramref name="credential"/> is null.</exception>
    public ApiContext(IEnvironment environment, BasicCredential credential) : this(environment)
    {
        Credential = credential ?? throw new ArgumentNullException(nameof(credential));
    }

    /// <summary>
    /// Gets or sets the access token associated with the API context.
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the basic credential associated with the API context.
    /// </summary>
    public BasicCredential Credential { get; set; }

    //"Bearer"
    /// <summary>
    /// Gets or sets the token type associated with the API context.
    /// </summary>
    public string TokenType { get; set; }

    /// <summary>
    /// Gets or sets the expiration time in seconds for the access token associated with the API context.
    /// </summary>
    public int ExpireIn { get; set; }

    /// <summary>
    /// Gets or sets the environment associated with the API context.
    /// </summary>
    public IEnvironment Environment { get; set; }
}