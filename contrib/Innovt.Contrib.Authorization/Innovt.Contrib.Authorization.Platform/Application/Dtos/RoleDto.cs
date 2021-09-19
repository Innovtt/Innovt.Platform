// Company: Antecipa
// Project: Innovt.Contrib.Authorization.Platform
// Solution: Innovt.Contrib.Authorization
// Date: 2021-06-02

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