// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Utilities;

namespace Innovt.Core.Caching;

/// <summary>
///     Represents a caching service interface for storing and retrieving data.
/// </summary>
/// <remarks>
///     This interface defines methods for retrieving and caching data, as well as removing cached data.
///     Implement this interface to create a caching service for your application.
/// </remarks>
public interface ICacheService : IDisposable
{
    /// <summary>
    ///     Gets the cached value associated with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the cached value.</typeparam>
    /// <param name="key">The unique identifier for the cached item.</param>
    /// <returns>The cached value if found; otherwise, the default value for the type.</returns>
    T GetValue<T>(string key);

    public async Task<T> GetValue<T>(string key, Func<CancellationToken, Task<T>> factory,
        CancellationToken cancellationToken)
    {
        if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        var value = GetValue<T>(key);

        if (value != null)
            return value;

        value = await factory(cancellationToken).ConfigureAwait(false);

        return value;
    }

    /// <summary>
    ///     Asynchronously retrieves a cached value associated with the specified key, or creates and caches it if not found.
    /// </summary>
    /// <typeparam name="T">The type of the cached value.</typeparam>
    /// <param name="key">The unique identifier for the cached item.</param>
    /// <param name="factory">A factory function to create the value if not found.</param>
    /// <param name="expiration">The time duration for which the value should be cached.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     The cached value if found, or a newly created value from the factory function if not found.
    /// </returns>
    public async Task<T> GetValueOrCreate<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan expiration,
        CancellationToken cancellationToken)
    {
        if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        var value = GetValue<T>(key);

        if (value is not null)
            return value;

        value = await factory(cancellationToken).ConfigureAwait(false);

        if (value is not null) SetValue(key, value, expiration);

        return value;
    }

    /// <summary>
    ///     Sets a value in the cache with the specified key and expiration duration.
    /// </summary>
    /// <typeparam name="T">The type of the value to be cached.</typeparam>
    /// <param name="key">The unique identifier for the cached item.</param>
    /// <param name="entity">The value to be cached.</param>
    /// <param name="expiration">The time duration for which the value should be cached.</param>
    void SetValue<T>(string key, T entity, TimeSpan expiration);

    /// <summary>
    ///     Removes a cached value associated with the specified key.
    /// </summary>
    /// <param name="key">The unique identifier for the cached item to be removed.</param>
    void Remove(string key);
}