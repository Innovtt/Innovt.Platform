// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Authorization.Platform
// Solution: Innovt.Platform
// Date: 2021-05-20
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;

namespace Innovt.Contrib.Authorization.Platform.Application.Dtos
{
    public class RoleDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IList<PermissionDto> Permissions { get; set; }

    }
}