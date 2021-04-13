// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain.Core
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

namespace Innovt.Domain.Core.Specification
{
    /// <summary>
    ///     Base class for composite specifications
    /// </summary>
    /// <typeparam name="TValueObject">Type of entity that check this specification</typeparam>
    public abstract class CompositeSpecification<TEntity>
        : Specification<TEntity>
        where TEntity : class
    {
        #region Properties

        /// <summary>
        ///     Left side specification for this composite element
        /// </summary>
        public abstract ISpecification<TEntity> LeftSideSpecification { get; }

        /// <summary>
        ///     Right side specification for this composite element
        /// </summary>
        public abstract ISpecification<TEntity> RightSideSpecification { get; }

        #endregion
    }
}