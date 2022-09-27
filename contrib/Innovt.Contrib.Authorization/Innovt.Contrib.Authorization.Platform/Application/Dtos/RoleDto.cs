// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System.Collections.Generic;
using System.Linq;
using Innovt.Domain.Security;

namespace Innovt.Contrib.Authorization.Platform.Application.Dtos;

public class RoleDto
{
    public string Name { get; set; }
    public string Scope { get; set; }


    public static RoleDto FromDomain(Role role)
    {
        return role is null ? null : new RoleDto { Name = role.Name, Scope = role.Scope };
    }

    public static List<RoleDto> FromDomain(IList<Role> roles)
    {
        return roles?.Select(FromDomain).ToList();
    }
}