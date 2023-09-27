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
/// Represents a group entity.
/// </summary>
public class Group : Entity<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Group"/> class.
    /// </summary>
    public Group()
    {
        CreatedAt = DateTimeOffset.UtcNow;
        Id = Guid.NewGuid();
    }
    /// <summary>
    /// Gets or sets the name of the group.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Gets or sets the description of the group.
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Gets the list of roles associated with the group.
    /// </summary>
    public IList<Role> Roles { get; private set; }
    /// <summary>
    /// Assigns a role to the group.
    /// </summary>
    /// <param name="role">The role to assign.</param>
    public void AssignRole(Role role)
    {
        if (role == null) throw new ArgumentNullException(nameof(role));

        Roles ??= new List<Role>();

        var exist = Roles.Any(r => r.Id == role.Id);

        if (exist)
            throw new BusinessException($"Role {role.Id} already allow to this group.");

        Roles.Add(role);
    }
    /// <summary>
    /// Unassigns a role from the group.
    /// </summary>
    /// <param name="role">The role to unassign.</param>
    public void UnAssignRole(Role role)
    {
        if (role == null) throw new ArgumentNullException(nameof(role));

        var roleToRemove = Roles?.SingleOrDefault(r => r.Id == role.Id);

        if (roleToRemove is null)
            throw new BusinessException($"Role {role.Id} is not linked to this group.");

        Roles.Remove(role);
    }
}