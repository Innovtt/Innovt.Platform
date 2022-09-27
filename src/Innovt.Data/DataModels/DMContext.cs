// Innovt Company
// Author: Michel Borges
// Project: Innovt.Data

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Innovt.Data.DataModels;

public class DMContext
{
    private static DMContext instance;
    private static object objlock = new();
    private Dictionary<string, IBaseDataModel> items;

    private DMContext()
    {
        items = new Dictionary<string, IBaseDataModel>();
    }

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

    public void Attach<T>(T entity) where T : IBaseDataModel
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        entity.EnableTrackingChanges = true;

        var key = entity.GetHashCode().ToString(CultureInfo.InvariantCulture);

        items.TryAdd(key, entity);
    }

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

    public T Find<T>(T entity) where T : IBaseDataModel
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        var key = entity.GetHashCode().ToString(CultureInfo.InvariantCulture);

        if (items.TryGetValue(key, out var item))
            return (T)item;

        return default;
    }

    public List<object> GetAllWithChanges()
    {
        return items.Where(i => i.Value.HasChanges).Select(i => i.Value as object).ToList();
    }
}