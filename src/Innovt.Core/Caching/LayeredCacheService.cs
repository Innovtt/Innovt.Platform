using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Utilities;
using System;
using System.Collections.Generic;

namespace Innovt.Core.Caching;

public class LayeredCacheService : ICacheService, IDisposable
{
    private readonly ILogger logger;
    private List<ICacheService> cacheServices;

    public LayeredCacheService(ICacheService cacheDefaultLayer, ILogger logger)
    {
        if (cacheDefaultLayer == null) throw new ArgumentNullException(nameof(cacheDefaultLayer));

        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.cacheServices = new List<ICacheService>() { cacheDefaultLayer };
    }

    public LayeredCacheService(ICacheService cacheDefaultLayer, ICacheService cacheSecondLayer, ILogger logger)
    {
        if (cacheDefaultLayer == null) throw new ArgumentNullException(nameof(cacheDefaultLayer));
        if (cacheSecondLayer == null) throw new ArgumentNullException(nameof(cacheSecondLayer));

        this.cacheServices = new List<ICacheService>() { cacheDefaultLayer, cacheSecondLayer };

        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    ~LayeredCacheService() => Dispose(false);


    public T GetValue<T>(string key)
    {
        if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));

        foreach (var cacheService in cacheServices)
        {
            try
            {
                var value = cacheService.GetValue<T>(key);

                if (value != null)
                    return value;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error getting the cache value for key {key} and provider {cacheService.GetType()}");
            }
        }

        return default;
    }

    public void SetValue<T>(string key, T entity, TimeSpan expiration)
    {
        if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));

        foreach (var cacheService in cacheServices)
        {
            try
            {
                cacheService.SetValue(key, entity, expiration);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error setting value for cache {cacheService.GetType()}");
            }
        }
    }

    public void Remove(string key)
    {
        if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));

        foreach (var cacheService in cacheServices)
        {
            try
            {
                cacheService.Remove(key);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error removing value for cache {cacheService.GetType()}");
            }
        }
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
        cacheServices = null;
        GC.SuppressFinalize(this);
    }

}