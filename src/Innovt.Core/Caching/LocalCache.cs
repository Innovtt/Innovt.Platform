using Innovt.Core.Utilities;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Innovt.Core.Caching;

public class LocalCache : ICacheService, IDisposable
{
    private readonly IMemoryCache memoryCache;

    public LocalCache(IMemoryCache memoryCache)
    {
        this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public T GetValue<T>(string key)
    {
        if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));

        return memoryCache.Get<T>(key);
    }

    ~LocalCache() => Dispose(false);

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

    bool disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (disposed || !disposing)
            return;

        Dispose();

        disposed = true;
    }

    public void Dispose()
    {
        memoryCache?.Dispose();
    }
}