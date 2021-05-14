// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using Innovt.Core.Exceptions;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Security
{
    /// <summary>
    /// Define a list of permissions that can be used 
    /// </summary>
    public class Role : Entity<Guid>
    {
        public Role()
        {
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public string Name { get; set; }

        public string Description { get; set; }

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
    }
}