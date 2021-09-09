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
    internal class RoleDataModel: DataModelBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Scope { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid RoleId { get; set; }

        // public IList<Permission> Permissions { get; set; }

        public RoleDataModel()
        {
            EntityType = "Role";
        }

        public static Role ToDomain(RoleDataModel roleDataModel)
        {
            if (roleDataModel is null)
                return null;

            return new Role()
            {
                Name = roleDataModel.Name,
                Id = roleDataModel.RoleId,
                CreatedAt = roleDataModel.CreatedAt, 
                Description = roleDataModel.Description
            };
        }



        public static RoleDataModel FromDomain(Role role)
       {
            if (role is null)
                return null;

            return new RoleDataModel()
            {
                Name = role.Name,
                Scope = role.Description,
                Description =  role.Description,
                CreatedAt = role.CreatedAt.GetValueOrDefault().UtcDateTime, 
                RoleId = role.Id,
                Id = "",
                Sk = ""
            };
        }
    }
}