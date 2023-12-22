// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using System;
using System.Linq.Expressions;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Core.Specification;

/// <summary>
/// Specification to filter entities by their primary key (Id).
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public class ByPkSpecification<T> : ISpecification<T> where T : Entity
{
    private readonly int id;

    /// <summary>
    /// Initializes a new instance of the <see cref="ByPkSpecification{T}"/> class.
    /// </summary>
    /// <param name="id">The primary key value to filter by.</param>
    public ByPkSpecification(int id)
    {
        this.id = id;
    }

    /// <summary>
    /// Gets or sets the page number for paginated results.
    /// </summary>
    public int? Page { get; set; }

    /// <summary>
    /// Gets or sets the page size for paginated results.
    /// </summary>
    public int? PageSize { get; set; }

    /// <summary>
    /// Constructs an expression to filter entities based on the primary key.
    /// </summary>
    /// <returns>An expression that satisfies the specification.</returns>
    public Expression<Func<T, bool>> SatisfiedBy()
    {
        var spec = new DirectSpecification<T>(e => e.Id == id);

        return spec.SatisfiedBy();
    }
}