// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Innovt.Core.Cqrs.Queries;

/// <summary>
/// Represents a simple filter with generic data.
/// </summary>
/// <typeparam name="T">The type of data used as filter criteria.</typeparam>
/// <remarks>
/// This class allows you to create simple filters with generic data that can be used to filter data in queries or operations.
/// The filter may include criteria specific to the data type <typeparamref name="T"/>.
/// </remarks>
public class SimpleFilter<T> : IFilter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleFilter{T}"/> class with the specified data.
    /// </summary>
    /// <param name="data">The filter criteria of type <typeparamref name="T"/>.</param>
    public SimpleFilter(T data)
    {
        Data = data;
    }

    /// <summary>
    /// Gets or sets the filter criteria of type <typeparamref name="T"/>.
    /// </summary>
    public T Data { get; set; }

    /// <inheritdoc />
    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Implement validation logic specific to this filter if needed.
        return new List<ValidationResult>();
    }
}