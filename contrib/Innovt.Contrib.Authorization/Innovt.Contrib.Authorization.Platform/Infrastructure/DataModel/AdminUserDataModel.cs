// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System;
using Innovt.Contrib.Authorization.Platform.Domain;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.DataModel;

/// <summary>
///     Represents a data model for an administrator user.
/// </summary>
internal class AdminUserDataModel : DataModelBase
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AdminUserDataModel" /> class.
    /// </summary>
    public AdminUserDataModel()
    {
        EntityType = "AdminUser";
    }

    /// <summary>
    ///     Gets or sets the user ID.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    ///     Gets or sets the name of the administrator user.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets the email of the administrator user.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///     Gets or sets the password hash of the administrator user.
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    ///     Gets or sets the creation date and time of the administrator user.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the last access date and time of the administrator user.
    /// </summary>
    public DateTime LastAccess { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the administrator user is enabled.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    ///     Converts a data model instance to an <see cref="AdminUser" />.
    /// </summary>
    /// <param name="userDataModel">The data model to convert.</param>
    /// <returns>An <see cref="AdminUser" /> instance.</returns>
    public static AdminUser ToUser(AdminUserDataModel userDataModel)
    {
        if (userDataModel is null)
            return null;

        return new AdminUser
        {
            Id = userDataModel.UserId,
            Name = userDataModel.Name,
            PasswordHash = userDataModel.PasswordHash,
            IsEnabled = userDataModel.IsEnabled,
            CreatedAt = userDataModel.CreatedAt,
            Email = userDataModel.Email,
            LastAccess = userDataModel.LastAccess
        };
    }

    /// <summary>
    ///     Converts an <see cref="AdminUser" /> instance to a data model.
    /// </summary>
    /// <param name="user">The <see cref="AdminUser" /> instance to convert.</param>
    /// <returns>A data model instance.</returns>
    public static AdminUserDataModel FromUser(AdminUser user)
    {
        if (user is null)
            return null;

        return new AdminUserDataModel
        {
            Id = $"MU#{user.Email}", //MU = masterUser
            Sk = "ADMINUSER",
            Name = user.Name,
            UserId = user.Id,
            CreatedAt = user.CreatedAt.UtcDateTime,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            IsEnabled = user.IsEnabled,
            LastAccess = user.LastAccess.UtcDateTime
        };
    }
}