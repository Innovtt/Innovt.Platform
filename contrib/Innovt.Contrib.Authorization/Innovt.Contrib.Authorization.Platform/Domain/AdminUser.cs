// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System;
using Innovt.Core.Utilities;
using Innovt.Domain.Core.Model;

namespace Innovt.Contrib.Authorization.Platform.Domain;
/// <summary>
/// Represents an administrator user.
/// </summary>
public class AdminUser : Entity<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AdminUser"/> class.
    /// </summary
    public AdminUser()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
    }
    /// <summary>
    /// Gets or sets the name of the administrator user.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Gets or sets the email of the administrator user.
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// Gets or sets the password hash of the administrator user.
    /// </summary>
    public string PasswordHash { get; set; }
    /// <summary>
    /// Gets or sets the last access timestamp of the administrator user.
    /// </summary>
    public DateTimeOffset LastAccess { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the administrator user is enabled.
    /// </summary>
    public bool IsEnabled { get; set; }
    /// <summary>
    /// Checks if the provided password matches the stored password hash.
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <returns>True if the password is valid; otherwise, false.</returns>
    public bool IsPasswordValid(string password)
    {
        return PasswordHash == password.Md5Hash();
    }
    /// <summary>
    /// Registers access for the administrator user by updating the last access timestamp.
    /// </summary>
    public void RegisterAccess()
    {
        LastAccess = DateTime.UtcNow;
    }
}