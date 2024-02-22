// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using Innovt.Core.Utilities;
using Microsoft.Extensions.Caching.Memory;

namespace Innovt.Core.Caching;

/// <summary>
///     Represents a local caching service that implements the <see cref="ICacheService" /> interface
///     using an in-memory cache provided by <see cref="IMemoryCache" />.
/// </summary>
/// <remarks>
///     This class provides a concrete implementation of the <see cref="ICacheService" /> interface using
///     an in-memory cache for storing and retrieving data. It is designed for local caching within an application.
/// </remarks>
public class LocalCache : ICacheService, IDisposable
{
    private readonly IMemoryCache memoryCache;

    private bool disposed;

    /// <summary>
    ///     Initializes a new instance of the <see cref="LocalCache" /> class with the specified <see cref="IMemoryCache" />.
    /// </summary>
    /// <param name="memoryCache">The in-memory cache implementation provided by <see cref="IMemoryCache" />.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="memoryCache" /> is null.</exception>
    public LocalCache(IMemoryCache memoryCache)
    {
        this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    /// <inheritdoc />
    public T GetValue<T>(string key)
    {
        if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));

        return memoryCache.Get<T>(key);
    }

    /// <inheritdoc />
    public void SetValue<T>(string key, T entity, TimeSpan expiration)
    {
        if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));

        if (entity is null)
            return;

        memoryCache.Set(key, entity, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        });
    }

    /// <inheritdoc />
    public void Remove(string key)
    {
        if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));

        memoryCache.Remove(key);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        memoryCache?.Dispose();
    }

    /// <summary>
    ///     Finalizes an instance of the <see cref="LocalCache" /> class.
    /// </summary>
    ~LocalCache()
    {
        Dispose(false);
    }

    /// <summary>
    ///     Releases the unmanaged resources used by the <see cref="LocalCache" /> class
    ///     and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">
    ///     <c>true</c> to release both managed and unmanaged resources;
    ///     <c>false</c> to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposed || !disposing)
            return;

        Dispose();

        disposed = true;
    }
}