using Innovt.Core.Collections;
using Innovt.Core.Exceptions;
using Innovt.Domain.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace Innovt.Domain.Security
{
    public class Policy : ValueObject
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IList<PolicyPermission> Permissions { get; set; }

        public void AddPermission(int permissionId)
        {
            if (Permissions.IsNullOrEmpty())
                Permissions = new List<PolicyPermission>();

            var exist = Permissions.Any(p => p.PermissionId == permissionId);

            if (exist)
                throw new BusinessException($"Permission {permissionId} alreary exist to this Polocy.");

            var permission = new PolicyPermission { PermissionId = permissionId, Policy = this };

            Permissions.Add(permission);
        }

        public void RemovePermission(int permissionId)
        {
            var permission = Permissions?.FirstOrDefault(p => p.PermissionId == permissionId);

            if (permission == null)
                throw new BusinessException($"Permission {permissionId} do not exist in this Policy.");

            Permissions.Remove(permission);
        }
    }
}