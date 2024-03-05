// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System.Collections.Generic;
using System.Linq;
using Innovt.Core.Utilities.Mapper;

namespace Innovt.Core.Collections;

/// <summary>
///     A collection of extension methods for common operations on collections and objects.
/// </summary>
/// <remarks>
///     This static class provides extension methods to simplify common operations on collections,
///     such as checking for null or empty, adding items, and merging dictionaries.
/// </remarks>
public static class Extensions
{
    /// <summary>
    ///     Checks if an <see cref="IList{T}" /> is null or empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The <see cref="IList{T}" /> to check.</param>
    /// <returns><c>true</c> if the list is null or empty; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrEmpty<T>(this IList<T> list)
    {
        return list == null || list.Count == 0;
    }

    /// <summary>
    ///     Checks if an <see cref="IEnumerable{T}" /> is null or empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <param name="enumerable">The <see cref="IEnumerable{T}" /> to check.</param>
    /// <returns><c>true</c> if the enumerable is null or empty; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable == null || enumerable.ToList().IsNullOrEmpty();
    }

    /// <summary>
    ///     Checks if an <see cref="ICollection{T}" /> is null or empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The <see cref="ICollection{T}" /> to check.</param>
    /// <returns><c>true</c> if the collection is null or empty; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
    {
        return collection == null || collection.Count == 0;
    }

    /// <summary>
    ///     Checks if an <see cref="IEnumerable{T}" /> is not null and not empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <param name="enumerable">The <see cref="IEnumerable{T}" /> to check.</param>
    /// <returns><c>true</c> if the enumerable is not null and not empty; otherwise, <c>false</c>.</returns>
    public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return !enumerable.IsNullOrEmpty();
    }


    /// <summary>
    ///     Checks if an <see cref="ICollection{T}" /> is not null and not empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The <see cref="ICollection{T}" /> to check.</param>
    /// <returns><c>true</c> if the collection is not null and not empty; otherwise, <c>false</c>.</returns>
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
        list ??= [];

        list.Add(value);

        return list;
    }

    /// <summary>
    ///     Adds a key-value pair to the dictionary and initializes it if it is null.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="dictionary">The <see cref="Dictionary{TKey, TValue}" /> to add to.</param>
    /// <param name="key">The key to add to the dictionary.</param>
    /// <param name="value">The value to add to the dictionary.</param>
    public static Dictionary<TKey, TValue> AddFluent<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
        TKey key, TValue value)
    {
        dictionary ??= [];

        dictionary.Add(key, value);

        return dictionary;
    }

    /// <summary>
    ///     Merges two dictionaries, adding values from the second dictionary to the first.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionaries.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionaries.</typeparam>
    /// <param name="dictionary">The target <see cref="Dictionary{TKey, TValue}" /> to merge into.</param>
    /// <param name="dictionarySecond">The source dictionary to merge from.</param>
    /// <returns>The updated dictionary after merging.</returns>
    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
        Dictionary<TKey, TValue> dictionarySecond)
    {
        dictionary ??= [];

        if (dictionarySecond == null) return dictionary;

        foreach (var item in dictionarySecond) dictionary.TryAdd(item.Key, item.Value);

        return dictionary;
    }

    /// <summary>
    ///     Using a simple mapper to map an page collection of objects to another page collection of objects
    /// </summary>
    /// <param name="pageCollection"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static PagedCollection<T2> MapToPagedCollection<T, T2>(this IPagedCollection<T> pageCollection)
        where T : class where T2 : class
    {
        if (pageCollection is null)
            return new PagedCollection<T2>();

        return new PagedCollection<T2>
        {
            Page = pageCollection.Page,
            PageSize = pageCollection.PageSize,
            TotalRecords = pageCollection.TotalRecords,
            Items = pageCollection.Items.MapToList<T2>()
        };
    }
}