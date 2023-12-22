// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

namespace Innovt.Domain.Core.Model;

/// <summary>
/// Represents an aggregate root in the domain model with a specific type for the identifier.
/// </summary>
/// <typeparam name="T">The type of the identifier.</typeparam>
public interface IAggregateRoot<T>
{
    /// <summary>
    /// Gets or sets the unique identifier for the aggregate root.
    /// </summary>
    public T Id { get; set; }
}

/// <summary>
/// Represents an aggregate root in the domain model with an integer identifier.
/// </summary>
public interface IAggregateRoot
{
    /// <summary>
    /// Gets or sets the unique identifier for the aggregate root.
    /// </summary>
    public int Id { get; set; }
}