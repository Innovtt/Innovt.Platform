// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

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