// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using System;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Users;
/// <summary>
/// Represents a base user entity.
/// </summary>
public class BaseUser : Entity
{
    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public virtual string FirstName { get; set; }
    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public virtual string LastName { get; set; }
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public virtual string Email { get; set; }
    /// <summary>
    /// Gets or sets the password associated with the user.
    /// </summary>
    public virtual string Password { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the user is active.
    /// </summary>
    public virtual bool IsActive { get; set; }
    /// <summary>
    /// Gets or sets the last access timestamp for the user.
    /// </summary>
    public DateTimeOffset? LastAccess { get; set; }
    /// <summary>
    /// Gets the full name of the user by combining the first and last names.
    /// </summary>
    public string Name => $"{FirstName} {LastName}";
}