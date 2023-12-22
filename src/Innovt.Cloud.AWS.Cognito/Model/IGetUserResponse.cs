// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Cognito

using System;
using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Cognito.Model;

/// <summary>
///     Represents the response interface for retrieving user information.
/// </summary>
public interface IGetUserResponse
{
    /// <summary>
    ///     Gets or sets the user's username.
    /// </summary>
    string UserName { get; set; }

    /// <summary>
    ///     Gets or sets the user's first name.
    /// </summary>
    string FirstName { get; set; }

    /// <summary>
    ///     Gets or sets the user's last name.
    /// </summary>
    string LastName { get; set; }

    /// <summary>
    ///     Gets or sets the status of the user.
    /// </summary>
    string Status { get; set; }

    /// <summary>
    ///     Gets or sets custom attributes associated with the user.
    /// </summary>
    Dictionary<string, string> CustomAttributes { get; set; }

    /// <summary>
    ///     Gets or sets the date and time when the user was created.
    /// </summary>
    DateTime UserCreateDate { get; set; }

    /// <summary>
    ///     Gets or sets the date and time when the user was last modified.
    /// </summary>
    DateTime UserLastModifiedDate { get; set; }
}