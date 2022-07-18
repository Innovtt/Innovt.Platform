// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Exceptions;
using Innovt.Domain.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Innovt.Domain.Security;

/// <summary>
///     Define a list of permissions that can be used
/// </summary>
public class Role : Entity<Guid>
{
    public Role()
    {
        CreatedAt = DateTimeOffset.UtcNow;
        Id = Guid.NewGuid();
    }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Scope { get; set; }

    public IList<Permission> Permissions { get; set; }

    public void AssignPermission(Permission permission)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        Permissions ??= new List<Permission>();

        var permissionExists = Permissions.Any(p => p.Id == permission.Id);

        if (permissionExists)
            throw new BusinessException("Permission already assigned.");

        Permissions.Add(permission);
    }

    public void RemovePermission(Guid permissionId)
    {
        var permission = Permissions?.SingleOrDefault(p => p.Id == permissionId);

        if (permission is null)
            throw new BusinessException("Permission not assigned to this group.");

        Permissions.Remove(permission);
    }

    public override bool Equals(object obj)
    {
        if (obj is not Role role)
            return false;

        return role.Name == Name && Scope == role.Scope;
    }
}