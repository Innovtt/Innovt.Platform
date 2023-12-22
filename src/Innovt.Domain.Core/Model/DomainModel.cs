// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Core

using System.Collections.Generic;
using System.Linq;

namespace Innovt.Domain.Core.Model;

/// <summary>
///     Represents a domain model containing a collection of value objects.
/// </summary>
/// <typeparam name="T">The type of value object.</typeparam>
public class DomainModel<T> : ValueObject where T : ValueObject
{
    private readonly List<T> models = new();

    /// <summary>
    ///     Adds a value object to the domain model.
    /// </summary>
    /// <param name="model">The value object to be added.</param>
    protected void AddModel(T model)
    {
        models.Add(model);
    }

    /// <summary>
    ///     Retrieves a list of all value objects in the domain model.
    /// </summary>
    /// <returns>A list of value objects.</returns>
    public List<T> FindAll()
    {
        return models;
    }

    /// <summary>
    ///     Retrieves a value object by its primary key.
    /// </summary>
    /// <param name="id">The primary key to search for.</param>
    /// <returns>The value object found, or null if not found.</returns>
    public T GetByPk(int id)
    {
        return models.SingleOrDefault(s => s.Id == id);
    }
}