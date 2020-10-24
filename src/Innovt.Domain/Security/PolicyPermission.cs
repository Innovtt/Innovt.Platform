using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Security
{
    public class PolicyPermission: ValueObject
    {
        public Permission Permission { get; set; }
        public int PermissionId { get; set; }

        public Policy Policy { get; set; }

        public int PolicyId { get; set; }


    }
}
