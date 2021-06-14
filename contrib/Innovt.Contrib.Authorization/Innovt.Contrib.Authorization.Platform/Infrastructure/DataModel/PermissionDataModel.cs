﻿// INNOVT TECNOLOGIA 2014-2021
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
            if (permission == null) throw new ArgumentNullException(nameof(permission));


            throw new NotImplementedException();
        }
    }
}