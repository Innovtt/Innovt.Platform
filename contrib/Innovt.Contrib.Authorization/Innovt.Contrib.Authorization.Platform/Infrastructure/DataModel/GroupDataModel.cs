// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-06-02

using System;
using System.Collections.Generic;
using Innovt.Domain.Security;

namespace Innovt.Contrib.Authorization.Platform.Infrastructure.DataModel
{
    internal class GroupDataModel : DataModelBase
    {
        public GroupDataModel()
        {
            EntityType = "Group";
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Scope { get; set; }

        public Guid GroupId { get; set; }

        public DateTime CreatedAt { get; set; }

        public IList<string> Users { get; private set; }

        public static GroupDataModel FromGroup(Group group)
        {
            if (group is null)
                return null;

            return new GroupDataModel
            {
                Name = group.Name,
                Description = group.Description,
                Id = $"G#{group.Name}",
                Sk = $"G#{group.Name}"
            };
        }

        public static Group ToGroup(GroupDataModel group)
        {
            if (group is null)
                return null;

            return new Group
            {
                Name = group.Name,
                Description = group.Description,
                Id = group.GroupId,
                CreatedAt = group.CreatedAt
            };
        }
    }
}