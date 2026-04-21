// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Dynamo

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Innovt.Cloud.AWS.Dynamo.ChangeTracking;

public sealed class ChangeTracker : IChangeTracker
{
    private static readonly object CycleSentinel = new();

    private readonly ConditionalWeakTable<object, object?[]> snapshots = new();

    public void Attach(object entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var metadata = EntityMetadata.For(entity.GetType());

        if (metadata.AllScalar)
        {
            var values = new object?[metadata.Getters.Length];
            for (var i = 0; i < values.Length; i++)
                values[i] = metadata.Getters[i](entity);
            snapshots.AddOrUpdate(entity, values);
            return;
        }

        var visited = new HashSet<object>(ReferenceEqualityComparer.Instance) { entity };
        snapshots.AddOrUpdate(entity, CaptureProperties(entity, visited));
    }

    public EntityState GetState(object entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (!snapshots.TryGetValue(entity, out var stored))
            return EntityState.Added;

        var metadata = EntityMetadata.For(entity.GetType());

        if (stored.Length != metadata.Getters.Length)
            return EntityState.Modified;

        if (metadata.AllScalar)
        {
            for (var i = 0; i < metadata.Getters.Length; i++)
            {
                var current = metadata.Getters[i](entity);
                var storedValue = stored[i];
                if (ReferenceEquals(current, storedValue)) continue;
                if (current is null || storedValue is null || !current.Equals(storedValue)) 
                    return EntityState.Modified;
            }

            return EntityState.Unchanged;
        }

        HashSet<object>? visited = null;
        for (var i = 0; i < metadata.Getters.Length; i++)
        {
            if (!ValueMatches(metadata.Getters[i](entity), stored[i], ref visited, entity))
                return EntityState.Modified;
        }

        return EntityState.Unchanged;
    }

    private static object?[] CaptureProperties(object entity, HashSet<object> visited)
    {
        var getters = EntityMetadata.For(entity.GetType()).Getters;
        var values = new object?[getters.Length];

        for (var i = 0; i < getters.Length; i++)
            values[i] = Capture(getters[i](entity), visited);

        return values;
    }

    private static object? Capture(object? value, HashSet<object> visited)
    {
        if (value is null)
            return null;

        var type = value.GetType();

        if (EntityMetadata.IsSimple(type))
            return value;

        if (!visited.Add(value))
            return CycleSentinel;

        try
        {
            if (value is IEnumerable enumerable)
                return CaptureEnumerable(enumerable, visited);

            return CaptureProperties(value, visited);
        }
        finally
        {
            visited.Remove(value);
        }
    }

    private static object?[] CaptureEnumerable(IEnumerable enumerable, HashSet<object> visited)
    {
        var items = new List<object?>();
        foreach (var item in enumerable)
            items.Add(Capture(item, visited));
        return items.ToArray();
    }

    private static bool ValueMatches(object? current, object? stored, ref HashSet<object>? visited, object root)
    {
        if (ReferenceEquals(current, stored)) return true;
        if (current is null || stored is null) return false;

        if (EntityMetadata.IsSimple(current.GetType()))
            return current.Equals(stored);

        if (stored is not object?[] storedArray)
        {
            if (!ReferenceEquals(stored, CycleSentinel))
                return false;
            visited ??= new HashSet<object>(ReferenceEqualityComparer.Instance) { root };
            return visited.Contains(current);
        }

        visited ??= new HashSet<object>(ReferenceEqualityComparer.Instance) { root };
        if (!visited.Add(current))
            return false;

        try
        {
            if (current is IEnumerable enumerable)
            {
                var index = 0;
                foreach (var item in enumerable)
                {
                    if (index >= storedArray.Length) return false;
                    if (!ValueMatches(item, storedArray[index], ref visited, root)) return false;
                    index++;
                }
                return index == storedArray.Length;
            }

            var getters = EntityMetadata.For(current.GetType()).Getters;
            if (storedArray.Length != getters.Length) return false;
            for (var i = 0; i < getters.Length; i++)
            {
                if (!ValueMatches(getters[i](current), storedArray[i], ref visited, root))
                    return false;
            }

            return true;
        }
        finally
        {
            visited?.Remove(current);
        }
    }
}
