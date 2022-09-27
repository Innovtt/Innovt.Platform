// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System.Collections.Generic;
using System.Linq;

namespace Innovt.Core.Collections;

public static class Extensions
{
    public static bool IsNullOrEmpty<T>(this IList<T> list)
    {
        return list == null || list.Count == 0;
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable == null || enumerable.ToList().IsNullOrEmpty();
    }

    public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
    {
        return collection == null || collection.Count == 0;
    }

    public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return !enumerable.IsNullOrEmpty();
    }

    public static bool IsNotNullOrEmpty<T>(this ICollection<T> collection)
    {
        return !collection.IsNullOrEmpty();
    }


    /// <summary>
    ///     Initialize the collection if is null, so you don't have to check it
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IList<T> AddFluent<T>(this IList<T> list, T value) where T : class
    {
        list ??= new List<T>();

        list.Add(value);

        return list;
    }

    public static Dictionary<TKey, TValue> AddFluent<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
        TKey key, TValue value)
    {
        dictionary ??= new Dictionary<TKey, TValue>();

        dictionary.Add(key, value);

        return dictionary;
    }

    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
        Dictionary<TKey, TValue> dictionarySecond)
    {
        dictionary ??= new Dictionary<TKey, TValue>();

        if (dictionarySecond == null) return dictionary;

        foreach (var item in dictionarySecond)
        {
            dictionary.TryAdd(item.Key, item.Value);
        }

        return dictionary;
    }
}