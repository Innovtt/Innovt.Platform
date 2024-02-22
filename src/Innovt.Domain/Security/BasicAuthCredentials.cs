// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

namespace Innovt.Domain.Security;

/// <summary>
///     Represents basic authentication credentials consisting of a username and password.
/// </summary>
public class BasicAuthCredentials
{
    /// <summary>
    ///     Gets or sets the username for basic authentication.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    ///     Gets or sets the password for basic authentication.
    /// </summary>
    public string Password { get; set; }
}