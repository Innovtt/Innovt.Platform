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
    ///     A Direct Specification is a simple implementation
    ///     of specification that acquire this from a lambda expression
    ///     in  constructor
    /// </summary>
    /// <typeparam name="TValueObject">Type of entity that check this specification</typeparam>
    public sealed class DirectSpecification<TEntity>
        : Specification<TEntity>
        where TEntity : class
    {
        #region Members

        private readonly Expression<Func<TEntity, bool>> matchingCriteria;

        #endregion

        #region Constructor

        /// <summary>
        ///     Default constructor for Direct Specification
        /// </summary>
        /// <param name="matchingCriteria">A Matching Criteria</param>
        public DirectSpecification(Expression<Func<TEntity, bool>> matchingCriteria)
        {
            this.matchingCriteria = matchingCriteria ?? throw new ArgumentNullException(nameof(matchingCriteria));
        }

        #endregion

        public static DirectSpecification<TEntity> By(Expression<Func<TEntity, bool>> matchingCriteria)
        {
            return new DirectSpecification<TEntity>(matchingCriteria);
        }

        #region Override

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            return matchingCriteria;
        }

        #endregion
    }
}