// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Address
{
    public class City : ValueObject
    {
        public string Name { get; set; }

        public int StateId { get; set; }

        public virtual State State { get; set; }
    }
}