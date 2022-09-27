// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using Innovt.Core.Utilities;
using Microsoft.Extensions.Caching.Memory;

namespace Innovt.Core.Caching;

public class LocalCache : ICacheService, IDisposable
{
    private readonly IMemoryCache memoryCache;

    private bool disposed;

    public LocalCache(IMemoryCache memoryCache)
    {
        this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public T GetValue<T>(string key)
    {
        if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));

        return memoryCache.Get<T>(key);
    }

    public void SetValue<T>(string key, T entity, TimeSpan expiration)
    {
        if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));

        if (entity is null)
            return;

        memoryCache.Set<T>(key, entity, new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = expiration
        });
    }

    public void Remove(string key)
    {
        if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));

        memoryCache.Remove(key);
    }

    public void Dispose()
    {
        memoryCache?.Dispose();
    }

    ~LocalCache()
    {
        Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed || !disposing)
            return;

        Dispose();

        disposed = true;
    }
}