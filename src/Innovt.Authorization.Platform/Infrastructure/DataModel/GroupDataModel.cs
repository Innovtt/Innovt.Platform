// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Authorization.Platform
// Solution: Innovt.Platform
// Date: 2021-05-14
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Cloud.Table;
using Innovt.Domain.Security;

namespace Innovt.Authorization.Platform.Infrastructure.DataModel
{
    internal class GroupDataModel:DataModelBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        //public IList<Role> Roles { get; private set; }

        //public IList<string> Users { get; private set; }
        public static GroupDataModel FromGroup(Group group)
        {
            if (@group == null) throw new ArgumentNullException(nameof(@group));
            throw new System.NotImplementedException();
        }
    }
}