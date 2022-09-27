// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Collections.Generic;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Utilities;

namespace Innovt.Core.Caching;

public class MultiLayerCacheService : ICacheService, IDisposable
{
    private readonly ILogger logger;
    private List<ICacheService> cacheServices;

    private bool disposed;

    public MultiLayerCacheService(ICacheService cacheDefaultLayer, ILogger logger)
    {
        if (cacheDefaultLayer == null) throw new ArgumentNullException(nameof(cacheDefaultLayer));

        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        cacheServices = new List<ICacheService>() { cacheDefaultLayer };
    }

    public MultiLayerCacheService(ICacheService cacheDefaultLayer, ICacheService cacheSecondLayer, ILogger logger)
    {
        if (cacheDefaultLayer == null) throw new ArgumentNullException(nameof(cacheDefaultLayer));
        if (cacheSecondLayer == null) throw new ArgumentNullException(nameof(cacheSecondLayer));

        cacheServices = new List<ICacheService>() { cacheDefaultLayer, cacheSecondLayer };

        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


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

    public void Dispose()
    {
        cacheServices = null;
        GC.SuppressFinalize(this);
    }


    ~MultiLayerCacheService()
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