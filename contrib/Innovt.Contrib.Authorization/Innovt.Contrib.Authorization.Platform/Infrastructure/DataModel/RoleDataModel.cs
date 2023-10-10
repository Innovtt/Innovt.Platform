// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System;
using Innovt.Domain.Security;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.DataModel;
/// <summary>
/// Represents a data model for a role.
/// </summary>
internal class RoleDataModel : DataModelBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleDataModel"/> class.
    /// </summary>
    public RoleDataModel()
    {
        EntityType = "Role";
    }
    /// <summary>
    /// Gets or sets the name of the role.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Gets or sets the description of the role.
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Gets or sets the scope of the role.
    /// </summary
    public string Scope { get; set; }
    /// <summary>
    /// Gets or sets the creation date and time of the role.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Gets or sets the role ID.
    /// </summary>
    public Guid RoleId { get; set; }
    /// <summary>
    /// Converts a data model instance to a <see cref="Role"/>.
    /// </summary>
    /// <param name="roleDataModel">The data model to convert.</param>
    /// <returns>A <see cref="Role"/> instance.</returns>
    public static Role ToDomain(RoleDataModel roleDataModel)
    {
        if (roleDataModel is null)
            return null;

        return new Role
        {
            Name = roleDataModel.Name,
            Scope = roleDataModel.Scope,
            Id = roleDataModel.RoleId,
            CreatedAt = roleDataModel.CreatedAt,
            Description = roleDataModel.Description
        };
    }
    /// <summary>
    /// Converts a <see cref="Role"/> instance to a data model.
    /// </summary>
    /// <param name="role">The <see cref="Role"/> instance to convert.</param>
    /// <returns>A data model instance.</returns>
    public static RoleDataModel FromDomain(Role role)
    {
        if (role is null)
            return null;

        return new RoleDataModel
        {
            Name = role.Name,
            Scope = role.Scope,
            Description = role.Description,
            CreatedAt = role.CreatedAt.GetValueOrDefault().UtcDateTime,
            RoleId = role.Id,
            Id = $"R#{role.Name}",
            Sk = $"S#{role.Scope}"
        };
    }
}