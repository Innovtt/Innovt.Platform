using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Security
{
    public class SecurityGroupPolicy : ValueObject
    {
        public SecurityGroup SecurityGroup { get; set; }

        public int SecurityGroupId { get; set; }

        public Policy Policy { get; set; }

        public int PolicyId { get; set; }
    }
}