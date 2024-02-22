// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

namespace Innovt.Domain.Core.Model;

/// <summary>
///     Represents a simple value object with a generic identifier type.
/// </summary>
/// <typeparam name="T">The type of the identifier.</typeparam>
public class SimpleVo<T> : ValueObject<T> where T : struct
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SimpleVo{T}" /> class.
    /// </summary>
    public SimpleVo()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SimpleVo{T}" /> class with the specified identifier and description.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="description">The description.</param>
    public SimpleVo(T id, string description)
    {
        Id = id;
        Description = description;
    }

    /// <summary>
    ///     Gets or sets the description of the value object.
    /// </summary>
    public string Description { get; set; }
}