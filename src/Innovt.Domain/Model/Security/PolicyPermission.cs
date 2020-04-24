using System;
using System.Collections.Generic;
using System.Text;

namespace Innovt.Domain.Model.Security
{
    public class PolicyPermission: ValueObject
    {
        public Permission Permission { get; set; }
        public int PermissionId { get; set; }

        public Policy Policy { get; set; }

        public int PolicyId { get; set; }


    }
}
