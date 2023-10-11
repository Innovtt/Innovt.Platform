// Innovt Company
// Author: Michel Borges
// Project: Innovt.Contrib.Authorization.Platform

using System.Collections.Generic;
using System.Linq;
using Innovt.Domain.Security;

namespace Innovt.Contrib.Authorization.Platform.Application.Dtos;
/// <summary>
/// Data transfer object (DTO) for a role.
/// </summary>
public class RoleDto
{
    /// <summary>
    /// Gets or sets the name of the role.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Gets or sets the scope of the role.
    /// </summary>
    public string Scope { get; set; }

    /// <summary>
    /// Creates a RoleDto instance from a domain Role object.
    /// </summary>
    /// <param name="role">The domain Role object.</param>
    /// <returns>The created RoleDto instance.</returns>
    public static RoleDto FromDomain(Role role)
    {
        return role is null ? null : new RoleDto { Name = role.Name, Scope = role.Scope };
    }
    /// <summary>
    /// Creates a list of RoleDto instances from a list of domain Role objects.
    /// </summary>
    /// <param name="roles">The list of domain Role objects.</param>
    /// <returns>The created list of RoleDto instances.</returns>
    public static List<RoleDto> FromDomain(IList<Role> roles)
    {
        return roles?.Select(FromDomain).ToList();
    }
}