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
using Innovt.Core.Utilities;
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

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

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
            if (Permissions.IsNull())
                return;
            
            var permission = Permissions.SingleOrDefault(p => p.Id == permissionId);

            if (permission.IsNull())
                throw new BusinessException("Permission not assigned to this group.");

            Permissions.Remove(permission);
        }
    }
}