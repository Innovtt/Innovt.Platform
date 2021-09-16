// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Authorization.Platform
// Solution: Innovt.Platform
// Date: 2021-05-14
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using Innovt.Domain.Security;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.DataModel
{
    internal class PermissionDataModel: DataModelBase
    {
        public string Scope { get; set; }

        public string Name { get; set; }

        public string Resource { get; set; }

        public Guid PermissionId { get; set; }

        public PermissionDataModel()
        {
            EntityType = "Permission";
        }
        
        public static PermissionDataModel FromPermission(Permission permission)
        {
            if (permission is null)
                return null;

            return new PermissionDataModel()
            {
                Name = permission.Name,
                Scope = permission.Scope,
                Resource = permission.Resource,
                PermissionId = permission.Id,                
                Sk = $"S#{permission.Scope}",
                Id = $"P#{permission.Resource}"
                //Sk = $"R#{permission.Resource}#N#{permission.Name}#",
                //Id = $"P#{permission.Id}"
            };
        }


        public static IList<Permission> ToDomain(IList<PermissionDataModel> permissions)
        {
            if (permissions is null)
                return null;

            return permissions.Select(ToDomain).ToList();
        }

        public static Permission ToDomain(PermissionDataModel permission)
        {
            if (permission is null)
                return null;

            return new Permission()
            {
                Name = permission.Name,
                Scope = permission.Scope,
                Resource = permission.Resource,
                Id  = permission.PermissionId
            };
        }
    }
}