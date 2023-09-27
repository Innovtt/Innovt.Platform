// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Innovt.Data.DataModels;
/// <summary>
/// Represents a context for managing instances of data models and tracking changes.
/// </summary>
public class DMContext
{
    private static DMContext instance;
    private static object objlock = new();
    private Dictionary<string, IBaseDataModel> items;
    /// <summary>
    /// Private constructor to ensure singleton pattern and initialize the context.
    /// </summary>
    private DMContext()
    {
        items = new Dictionary<string, IBaseDataModel>();
    }
    /// <summary>
    /// Gets the singleton instance of the DMContext.
    /// </summary>
    /// <returns>The singleton instance of the DMContext.</returns>
    public static DMContext Instance()
    {
        lock (objlock)
        {
            if (instance is null)
            {
                instance = new DMContext();
            }
        }

        return instance;
    }
    /// <summary>
    /// Attaches a data model entity to the context for change tracking.
    /// </summary>
    /// <typeparam name="T">The type of the entity implementing IBaseDataModel.</typeparam>
    /// <param name="entity">The entity to be attached.</param>
    /// <exception cref="ArgumentNullException">Thrown when the entity is null.</exception>
    public void Attach<T>(T entity) where T : IBaseDataModel
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        entity.EnableTrackingChanges = true;

        var key = entity.GetHashCode().ToString(CultureInfo.InvariantCulture);

        items.TryAdd(key, entity);
    }

    /// <summary>
    /// Detaches a data model entity from the context and disables change tracking.
    /// </summary>
    /// <typeparam name="T">The type of the entity implementing IBaseDataModel.</typeparam>
    /// <param name="entity">The entity to be detached.</param>
    /// <exception cref="ArgumentNullException">Thrown when the entity is null.</exception>
    public void DeAttach<T>(T entity) where T : IBaseDataModel
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        entity.EnableTrackingChanges = true;

        var key = entity.GetHashCode().ToString(CultureInfo.InvariantCulture);

        if (items.TryGetValue(key, out var item))
        {
            item.EnableTrackingChanges = false;

            items.Remove(key);
        }
    }
    /// <summary>
    /// Finds a data model entity in the context based on its hash code.
    /// </summary>
    /// <typeparam name="T">The type of the entity implementing IBaseDataModel.</typeparam>
    /// <param name="entity">The entity to be found.</param>
    /// <returns>The found data model entity or default if not found.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the entity is null.</exception>
    public T Find<T>(T entity) where T : IBaseDataModel
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        var key = entity.GetHashCode().ToString(CultureInfo.InvariantCulture);

        if (items.TryGetValue(key, out var item))
            return (T)item;

        return default;
    }
    /// <summary>
    /// Retrieves a list of data model entities that have changes.
    /// </summary>
    /// <returns>A list of data model entities with pending changes.</returns>
    public List<object> GetAllWithChanges()
    {
        return items.Where(i => i.Value.HasChanges).Select(i => i.Value as object).ToList();
    }
}