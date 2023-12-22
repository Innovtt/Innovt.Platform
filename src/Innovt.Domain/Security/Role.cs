// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain

using System;
using System.Collections.Generic;
using System.Linq;
using Innovt.Core.Exceptions;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Security;

/// <summary>
///     Define a list of permissions that can be used
/// </summary>
public class Role : Entity<Guid>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Role" /> class.
    /// </summary>
    public Role()
    {
        CreatedAt = DateTimeOffset.UtcNow;
        Id = Guid.NewGuid();
    }

    /// <summary>
    ///     Gets or sets the name of the role.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets the description of the role.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    ///     Gets or sets the scope of the role.
    /// </summary>
    public string Scope { get; set; }

    /// <summary>
    ///     Gets or sets the list of permissions associated with the role.
    /// </summary>
    public IList<Permission> Permissions { get; set; }

    /// <summary>
    ///     Assigns a permission to the role.
    /// </summary>
    /// <param name="permission">The permission to be assigned.</param>
    public void AssignPermission(Permission permission)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        Permissions ??= new List<Permission>();

        var permissionExists = Permissions.Any(p => p.Id == permission.Id);

        if (permissionExists)
            throw new BusinessException("Permission already assigned.");

        Permissions.Add(permission);
    }

    /// <summary>
    ///     Removes a permission from the role by its identifier.
    /// </summary>
    /// <param name="permissionId">The identifier of the permission to be removed.</param>
    public void RemovePermission(Guid permissionId)
    {
        var permission = Permissions?.SingleOrDefault(p => p.Id == permissionId);

        if (permission is null)
            throw new BusinessException("Permission not assigned to this group.");

        Permissions.Remove(permission);
    }

    /// <summary>
    ///     Determines whether the current <see cref="Role" /> instance is equal to another object.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>
    ///     <c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object obj)
    {
        if (obj is not Role role)
            return false;

        return role.Name == Name && Scope == role.Scope;
    }
}