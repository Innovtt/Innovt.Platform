// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System.Text.Json.Serialization;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
///     Gets or sets the error message associated with the sign-in response, if any.
/// </summary>
public class OAuth2SignInResponse : SignInResponse
{
    [JsonPropertyName("error")] public string Error { get; set; }

    /// <summary>
    ///     Gets or sets a flag indicating whether the user needs to register.
    /// </summary>
    public bool NeedRegister { get; set; }

    /// <summary>
    ///     Gets or sets the first name of the user.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    ///     Gets or sets the last name of the user.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    ///     Gets or sets the URL of the user's profile picture.
    /// </summary>
    public string Picture { get; set; }

    /// <summary>
    ///     Gets or sets the email address of the user.
    /// </summary>
    public string Email { get; set; }
}