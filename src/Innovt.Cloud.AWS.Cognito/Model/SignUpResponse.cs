// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
/// Represents the response from a user sign-up operation.
/// </summary>
public class SignUpResponse
{
    /// <summary>
    /// Gets or sets a value indicating whether the user's sign-up has been confirmed.
    /// </summary>
    public bool Confirmed { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier (UUID) associated with the user.
    /// </summary>
    public string UUID { get; set; }
}