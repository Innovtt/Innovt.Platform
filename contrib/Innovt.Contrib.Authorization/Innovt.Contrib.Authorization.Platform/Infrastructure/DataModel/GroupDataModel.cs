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

        public bool IsActive { get; set; }

        public GroupDataModel()
        {
            EntityType = "Group";
        }

        //public IList<Role> Roles { get; private set; }

        public string BuildPk()
        {
            return $"G#{Name}";
        }

        public IList<string> Users { get; private set; }

        public static GroupDataModel FromGroup(Group group)
        {
            if (@group == null) throw new ArgumentNullException(nameof(@group));


            group.Roles[0].Permissions[0].


            throw new System.NotImplementedException();
        }
    }
}