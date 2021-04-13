// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain.Core
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Linq.Expressions;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Core.Specification
{
    public class ByPkSpecification<T> : ISpecification<T> where T : Entity
    {
        private readonly int id;

        public ByPkSpecification(int id)
        {
            this.id = id;
        }

        public int? Page { get; set; }
        public int? PageSize { get; set; }

        public Expression<Func<T, bool>> SatisfiedBy()
        {
            var spec = new DirectSpecification<T>(e => e.Id == id);

            return spec.SatisfiedBy();
        }
    }
}