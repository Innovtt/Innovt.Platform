//===================================================================================
// Microsoft Developer & Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================


using System;
using System.Linq.Expressions;

namespace Innovt.Domain.Core.Specification
{
    /// <summary>
    /// A logic AND Specification
    /// </summary>
    /// <typeparam name="T">Type of entity that check this specification</typeparam>
    public sealed class AndSpecification<T>
        : CompositeSpecification<T>
        where T : class
    {
        #region Members

        private readonly ISpecification<T> rightSideSpecification = null;
        private readonly ISpecification<T> leftSideSpecification = null;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Default constructor for AndSpecification
        /// </summary>
        /// <param name="leftSide">Left side specification</param>
        /// <param name="rightSide">Right side specification</param>
        public AndSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            this.leftSideSpecification = leftSide ?? throw new ArgumentNullException("leftSide");
            this.rightSideSpecification = rightSide ?? throw new ArgumentNullException("rightSide");
        }

        #endregion

        #region Composite Specification overrides

        /// <summary>
        /// Left side specification
        /// </summary>
        public override ISpecification<T> LeftSideSpecification
        {
            get { return leftSideSpecification; }
        }

        /// <summary>
        /// Right side specification
        /// </summary>
        public override ISpecification<T> RightSideSpecification
        {
            get { return rightSideSpecification; }
        }

        /// <summary>
        /// <see cref="ISpecification{TEntity}"/>
        /// </summary>
        /// <returns><see cref="ISpecification{TEntity}"/></returns>
        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            Expression<Func<T, bool>> left = leftSideSpecification.SatisfiedBy();
            Expression<Func<T, bool>> right = rightSideSpecification.SatisfiedBy();

            return (left.And(right));
        }

        #endregion
    }
}