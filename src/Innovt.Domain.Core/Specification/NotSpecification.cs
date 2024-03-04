﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using System;
using System.Linq;
using System.Linq.Expressions;

namespace Innovt.Domain.Core.Specification;

/// <summary>
///     NotEspecification convert a original
///     specification with NOT logic operator
/// </summary>
/// <typeparam name="TValueObject">Type of element for this specificaiton</typeparam>
public sealed class NotSpecification<TEntity>
    : Specification<TEntity>
    where TEntity : class
{
    #region Members

    private readonly Expression<Func<TEntity, bool>> originalCriteria;

    #endregion

    #region Override Specification methods

    /// <summary>
    ///     <see cref="ISpecification{TEntity}" />
    /// </summary>
    /// <returns>
    ///     <see cref="ISpecification{TEntity}" />
    /// </returns>
    public override Expression<Func<TEntity, bool>> SatisfiedBy()
    {
        return Expression.Lambda<Func<TEntity, bool>>(Expression.Not(originalCriteria.Body),
            originalCriteria.Parameters.Single());
    }

    #endregion

    #region Constructor

    /// <summary>
    ///     Constructor for NotSpecificaiton
    /// </summary>
    /// <param name="originalSpecification">Original specification</param>
    public NotSpecification(ISpecification<TEntity> originalSpecification)
    {
        if (originalSpecification == null)
            throw new ArgumentNullException(nameof(originalSpecification));

        originalCriteria = originalSpecification.SatisfiedBy();
    }

    /// <summary>
    ///     Constructor for NotSpecification
    /// </summary>
    /// <param name="originalSpecification">Original specificaiton</param>
    public NotSpecification(Expression<Func<TEntity, bool>> originalSpecification)
    {
        originalCriteria = originalSpecification ?? throw new ArgumentNullException(nameof(originalSpecification));
    }

    #endregion
}