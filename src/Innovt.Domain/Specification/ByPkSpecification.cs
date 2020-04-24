using Innovt.Domain.Model;
using System;
using System.Linq.Expressions;

namespace Innovt.Domain.Specification
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
