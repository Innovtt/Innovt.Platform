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

        public DateTime CreatedAt { get; set; }

       // public IList<Permission> Permissions { get; set; }

       public static RoleDataModel FromRole(Role role)
       {
           if (role == null) throw new ArgumentNullException(nameof(role));
           throw new NotImplementedException();
       }
    }
}