using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Security
{
    public  class SecurityGroupUser : ValueObject
    {
        public int UserId { get; set; }

        //public BaseUser User { get; set; }

        public SecurityGroup SecurityGroup { get; set; }

        public int SecurityGroupId { get; set; }
    }
}
