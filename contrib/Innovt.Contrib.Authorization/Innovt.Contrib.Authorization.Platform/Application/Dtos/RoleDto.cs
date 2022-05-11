// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-09-22

using Innovt.Domain.Security;
using System.Collections.Generic;
using System.Linq;

namespace Innovt.Contrib.Authorization.Platform.Application.Dtos;

public class RoleDto
{
    public string Name { get; set; }
    public string Scope { get; set; }


    public static RoleDto FromDomain(Role role)
    {
        return role is null ? null : new RoleDto() { Name = role.Name, Scope = role.Scope };
    }


    public static List<RoleDto> FromDomain(IList<Role> roles)
    {
        if (roles is null)
            return null;

        return roles.Select(FromDomain).ToList();
    }
}