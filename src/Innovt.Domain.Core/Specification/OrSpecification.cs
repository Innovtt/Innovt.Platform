// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using System.Linq.Expressions;

namespace Innovt.Domain.Core.Specification
{
    /// <summary>
    ///     A Logic OR Specification
    /// </summary>
    /// <typeparam name="T">Type of entity that check this specification</typeparam>
    public sealed class OrSpecification<T>
        : CompositeSpecification<T>
        where T : class
    {
        #region Public Constructor

        /// <summary>
        ///     Default constructor for AndSpecification
        /// </summary>
        /// <param name="leftSide">Left side specification</param>
        /// <param name="rightSide">Right side specification</param>
        public OrSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            LeftSideSpecification = leftSide ?? throw new ArgumentNullException(nameof(leftSide));
            RightSideSpecification = rightSide ?? throw new ArgumentNullException(nameof(rightSide));
        }

        #endregion

        #region Members

        #endregion

        #region Composite Specification overrides

        /// <summary>
        ///     Left side specification
        /// </summary>
        public override ISpecification<T> LeftSideSpecification { get; }

        /// <summary>
        ///     Righ side specification
        /// </summary>
        public override ISpecification<T> RightSideSpecification { get; }

        /// <summary>
        ///     <see cref="ISpecification{TEntity}" />
        /// </summary>
        /// <returns>
        ///     <see cref="ISpecification{TEntity}" />
        /// </returns>
        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            var left = LeftSideSpecification.SatisfiedBy();
            var right = RightSideSpecification.SatisfiedBy();

            return left.Or(right);
        }

        #endregion
    }
}