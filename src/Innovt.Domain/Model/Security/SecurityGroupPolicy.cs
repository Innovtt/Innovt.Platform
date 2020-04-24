using System;
using System.Collections.Generic;
using System.Text;

namespace Innovt.Domain.Model.Security
{
    public class SecurityGroupPolicy : ValueObject
    {
        public SecurityGroup SecurityGroup { get; set; }

        public int SecurityGroupId { get; set; }

        public Policy Policy { get; set; }

        public int PolicyId { get; set; }
    }
}
