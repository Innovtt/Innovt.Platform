// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Authorization.Platform
// Solution: Innovt.Platform
// Date: 2021-05-14
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Domain.Security;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.DataModel
{
    internal class PermissionDataModel: DataModelBase
    {
        public string Domain { get; set; }

        public string Name { get; set; }

        public string Resource { get; set; }

        public Guid PermissionId { get; set; }

        public string BuildPk()
        {
            return $"P#{Id}";
        }

        public string BuildSk()
        {
            return $"R#{Resource}#N#{Name}#";
        }


        public static PermissionDataModel FromPermission(Permission permission)
        {
            if (permission is null)
                return null;

            return new PermissionDataModel()
            {
                Name = permission.Name,
                Domain = permission.Domain,
                Resource = permission.Resource,
                Sk = $"R#{permission.Resource}#N#{permission.Name}#",
                Id = $"P#{permission.Id}"
            };
        }

        public static Permission ToPermission(PermissionDataModel permission)
        {
            if (permission is null)
                return null;

            return new Permission()
            {
                Name = permission.Name,
                Domain = permission.Domain,
                Resource = permission.Resource,
                Id  = permission.PermissionId
            };
        }
    }
}