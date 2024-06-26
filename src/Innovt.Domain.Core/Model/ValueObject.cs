// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using System;
using System.Collections.Generic;

namespace Innovt.Domain.Core.Model;

/// <summary>
///     Represents a base class for value objects.
/// </summary>
public abstract class ValueObject<T> where T : struct
{
    /// <summary>
    ///     Gets or sets the identifier for the value object.
    /// </summary>
    public T Id { get; set; }

    /// <summary>
    ///     Determines whether the current value object is equal to another object.
    /// </summary>
    /// <param name="obj">The object to compare with the current value object.</param>
    /// <returns><c>true</c> if the objects are considered equal; otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is null) return false;

        return obj is ValueObject<T> @object &&
            EqualityComparer<T>.Default.Equals(Id, @object.Id);
    }

    /// <summary>
    ///     Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current value object.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}

/// <summary>
///     Represents a base class for value objects with a generic identifier type.
/// </summary>
/// <typeparam name="T">The type of the identifier.</typeparam>
public abstract class ValueObject : ValueObject<int>
{
}