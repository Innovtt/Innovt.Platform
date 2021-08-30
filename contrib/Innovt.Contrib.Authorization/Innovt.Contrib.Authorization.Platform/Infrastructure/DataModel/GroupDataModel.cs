// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Authorization.Platform
// Solution: Innovt.Platform
// Date: 2021-05-14
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Collections.Generic;
using Innovt.Domain.Security;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.DataModel
{
    internal class GroupDataModel:DataModelBase
    {
        public string Name { get; set; }

        public string Description { get; set; }


        public string Domain { get; set; }


        public Guid GroupId { get; set; }

        public DateTime CreatedAt { get; set; }

        public GroupDataModel()
        {
            EntityType = "Group";
        }

        public IList<string> Users { get; private set; }

        public static GroupDataModel FromGroup(Group group)
        {
            if (group is null)
                return null;

            return new GroupDataModel()
            {
                Name = group.Name,
                Domain = group.Domain,
                Description = group.Description,                
                Id = $"G#{group.Name}",
                Sk = $"G#{group.Name}",
            };
        }

        public static Group ToGroup(GroupDataModel group)
        {
            if (group is null)
                return null;

            return new Group()
            {
                Name = group.Name,
                Description = group.Description,                
                Id = group.GroupId,
                CreatedAt = group.CreatedAt,
                Domain = group.Domain,
            };
        }
    }
}