// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core

using System;
using System.Collections.Generic;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Utilities;

namespace Innovt.Core.Caching;

/// <summary>
///     Represents a multi-layer caching service that implements the <see cref="ICacheService" /> interface.
/// </summary>
/// <remarks>
///     This class provides a caching service that supports multiple caching layers. It allows data retrieval and storage
///     through a series of cache layers, falling back to subsequent layers if data is not found in earlier layers.
/// </remarks>
public sealed class MultiLayerCacheService : ICacheService, IDisposable
{
    private readonly ILogger logger;
    private List<ICacheService> cacheServices;

    private bool disposed;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MultiLayerCacheService" /> class with a default caching layer.
    /// </summary>
    /// <param name="cacheDefaultLayer">The default caching layer implementing <see cref="ICacheService" />.</param>
    /// <param name="logger">The logger implementation provided by <see cref="ILogger" />.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="cacheDefaultLayer" /> or <paramref name="logger" />
    ///     is null.
    /// </exception>
    public MultiLayerCacheService(ICacheService cacheDefaultLayer, ILogger logger)
    {
        if (cacheDefaultLayer == null) throw new ArgumentNullException(nameof(cacheDefaultLayer));

        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        cacheServices = new List<ICacheService> { cacheDefaultLayer };
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MultiLayerCacheService" /> class with multiple caching layers.
    /// </summary>
    /// <param name="cacheDefaultLayer">The default caching layer implementing <see cref="ICacheService" />.</param>
    /// <param name="cacheSecondLayer">The secondary caching layer implementing <see cref="ICacheService" />.</param>
    /// <param name="logger">The logger implementation provided by <see cref="ILogger" />.</param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="cacheDefaultLayer" />, <paramref name="cacheSecondLayer" />,
    ///     or <paramref name="logger" /> is null.
    /// </exception>
    public MultiLayerCacheService(ICacheService cacheDefaultLayer, ICacheService cacheSecondLayer, ILogger logger)
    {
        if (cacheDefaultLayer == null) throw new ArgumentNullException(nameof(cacheDefaultLayer));
        if (cacheSecondLayer == null) throw new ArgumentNullException(nameof(cacheSecondLayer));

        cacheServices = new List<ICacheService> { cacheDefaultLayer, cacheSecondLayer };

        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public T GetValue<T>(string key)
    {
        if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));

        foreach (var cacheService in cacheServices)
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

        return default;
    }

    /// <inheritdoc />
    public void SetValue<T>(string key, T entity, TimeSpan expiration)
    {
        if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));

        foreach (var cacheService in cacheServices)
            try
            {
                cacheService.SetValue(key, entity, expiration);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error setting value for cache {cacheService.GetType()}");
            }
    }

    /// <inheritdoc />
    public void Remove(string key)
    {
        if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));

        foreach (var cacheService in cacheServices)
            try
            {
                cacheService.Remove(key);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error removing value for cache {cacheService.GetType()}");
            }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        cacheServices = null;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Finalizes an instance of the <see cref="MultiLayerCacheService" /> class.
    /// </summary>
    ~MultiLayerCacheService()
    {
        Dispose(false);
    }

    /// <summary>
    ///     Releases the unmanaged resources used by the <see cref="MultiLayerCacheService" /> class
    ///     and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">
    ///     <c>true</c> to release both managed and unmanaged resources;
    ///     <c>false</c> to release only unmanaged resources.
    /// </param>
    private void Dispose(bool disposing)
    {
        if (disposed || !disposing)
            return;

        Dispose();

        disposed = true;
    }
}