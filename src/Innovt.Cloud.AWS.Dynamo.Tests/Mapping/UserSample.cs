using System;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Mapping;

/// <summary>
///     Represents a user in the application.
/// </summary>
public class UserSample
{
    /// <summary>
    ///     Gets or sets the unique identifier for the user.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the name of the user.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets the email address of the user.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///     Gets or sets the date and time when the user was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
}