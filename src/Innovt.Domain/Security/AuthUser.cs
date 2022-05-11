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

public class AuthUser : Entity
{
    public AuthUser()
    {
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public new string Id { get; set; }

    public string DomainId { get; set; }

    public string Name { get; set; }

    public IList<Group> Groups { get; private set; }

    public IList<Role> Roles { get; private set; }

    public void AssignRole(Role role)
    {
        if (role == null) throw new ArgumentNullException(nameof(role));

        Roles ??= new List<Role>();

        var roleExist = Roles.Any(r => r.Scope == role.Scope && r.Name == role.Name);

        if (roleExist)
            return;

        Roles.Add(role);
    }

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

    public void AssignGroup(Group group)
    {
        if (group == null) throw new ArgumentNullException(nameof(group));

        Groups ??= new List<Group>();

        var roleExist = Groups.Any(r => r.Name == group.Name);

        if (roleExist)
            throw new BusinessException($"Group {group.Name} already assigned.");

        Groups.Add(group);
    }

    public void UnAssignGroup(string groupName)
    {
        var group = Groups?.SingleOrDefault(r => r.Name == groupName);

        if (group is null)
            return;

        Groups.Remove(group);
    }
}