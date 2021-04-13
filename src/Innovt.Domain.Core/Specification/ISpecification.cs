// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain.Core
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Linq.Expressions;

namespace Innovt.Domain.Core.Specification
{
    /// <summary>
    ///     Base contract for Specification pattern, for more information
    ///     about this pattern see http://martinfowler.com/apsupp/spec.pdf
    ///     or http://en.wikipedia.org/wiki/Specification_pattern.
    ///     This is really a variant implementation where we have added Linq and
    ///     lambda expression into this pattern.
    /// </summary>
    public interface ISpecification<TEntity>
        where TEntity : class
    {
        int? Page { get; set; }
        int? PageSize { get; set; }

        /// <summary>
        ///     Check if this specification is satisfied by a
        ///     specific expression lambda
        /// </summary>
        /// <returns></returns>
        Expression<Func<TEntity, bool>> SatisfiedBy();
    }
}