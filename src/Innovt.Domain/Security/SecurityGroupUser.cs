// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Security
{
    public class SecurityGroupUser : ValueObject
    {
        public int UserId { get; set; }

        //public BaseUser User { get; set; }

        public SecurityGroup SecurityGroup { get; set; }

        public int SecurityGroupId { get; set; }
    }
}