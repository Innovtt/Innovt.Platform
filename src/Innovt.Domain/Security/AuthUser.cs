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
///     Represents a user in the authentication system.
/// </summary>
public class AuthUser : Entity
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AuthUser" /> class.
    /// </summary>
    public AuthUser()
    {
        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    ///     Gets or sets the user ID.
    /// </summary>
    public new string Id { get; set; }

    /// <summary>
    ///     Gets or sets the domain ID associated with the user.
    /// </summary>
    public string DomainId { get; set; }

    /// <summary>
    ///     Gets or sets the name of the user.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets the groups associated with the user.
    /// </summary>
    public IList<Group> Groups { get; private set; }

    /// <summary>
    ///     Gets or sets the roles associated with the user.
    /// </summary>
    public IList<Role> Roles { get; private set; }

    /// <summary>
    ///     Assigns a role to the user.
    /// </summary>
    /// <param name="role">The role to assign.</param>
    public void AssignRole(Role role)
    {
        if (role == null) throw new ArgumentNullException(nameof(role));

        Roles ??= new List<Role>();

        var roleExist = Roles.Any(r => r.Scope == role.Scope && r.Name == role.Name);

        if (roleExist)
            return;

        Roles.Add(role);
    }

    /// <summary>
    ///     Unassigns a role from the user.
    /// </summary>
    /// <param name="scope">The scope of the role.</param>
    /// <param name="roleName">The name of the role.</param>
    public void UnAssignRole(string scope, string roleName)
    {
        if (scope == null) throw new ArgumentNullException(nameof(scope));
        if (roleName == null) throw new ArgumentNullException(nameof(roleName));

        var role = Roles?.SingleOrDefault(r => r.Scope.Equals(scope, StringComparison.OrdinalIgnoreCase)
                                               && r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));

        if (role is null)
            return;

        Roles.Remove(role);
    }

    /// <summary>
    ///     Assigns a group to the user.
    /// </summary>
    /// <param name="group">The group to assign.</param>
    public void AssignGroup(Group group)
    {
        if (group == null) throw new ArgumentNullException(nameof(group));

        Groups ??= new List<Group>();

        var roleExist = Groups.Any(r => r.Name == group.Name);

        if (roleExist)
            throw new BusinessException($"Group {group.Name} already assigned.");

        Groups.Add(group);
    }

    /// <summary>
    ///     Unassigns a group from the user.
    /// </summary>
    /// <param name="groupName">The name of the group.</param>
    public void UnAssignGroup(string groupName)
    {
        var group = Groups?.SingleOrDefault(r => r.Name == groupName);

        if (group is null)
            return;

        Groups.Remove(group);
    }
}