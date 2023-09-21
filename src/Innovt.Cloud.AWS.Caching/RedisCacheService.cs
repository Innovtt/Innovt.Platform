// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Caching

using System;
using System.Diagnostics;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.Caching;
using Innovt.Core.CrossCutting.Log;
using ServiceStack.Redis;

namespace Innovt.Cloud.AWS.Caching;

/// <summary>
/// Represents a caching service that uses Redis as the cache provider.
/// </summary>
public class RedisCacheService : AwsBaseService, ICacheService
{
    protected static readonly ActivitySource RedisProviderActivitySource =
        new("Innovt.Cloud.AWS.Caching.RedisCacheService");

    private readonly PooledRedisClientManager redisClientManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="RedisCacheService"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging.</param>
    /// <param name="configuration">The AWS configuration.</param>
    /// <param name="providerConfiguration">The configuration for the Redis cache provider.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="providerConfiguration"/> is null.</exception>
    public RedisCacheService(ILogger logger, IAwsConfiguration configuration,
        RedisProviderConfiguration providerConfiguration) : base(logger, configuration)
    {
        if (providerConfiguration == null) throw new ArgumentNullException(nameof(providerConfiguration));

        redisClientManager =
            new PooledRedisClientManager(providerConfiguration.ReadWriteHosts, providerConfiguration.ReadOnlyHosts)
            {
                ConnectTimeout = providerConfiguration.ConnectTimeout,
                PoolTimeout = providerConfiguration.PoolTimeOutInSeconds
            };
    }

    /// <summary>
    /// Gets a cached value of type <typeparamref name="T"/> associated with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the cached value.</typeparam>
    /// <param name="key">The key associated with the cached value.</param>
    /// <returns>The cached value if found; otherwise, the default value for <typeparamref name="T"/>.</returns>
    public T GetValue<T>(string key)
    {
        using var client = redisClientManager.GetClient();

        return client.Get<T>(key);
    }

    /// <summary>
    /// Sets a cached value of type <typeparamref name="T"/> associated with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to cache.</typeparam>
    /// <param name="key">The key to associate with the cached value.</param>
    /// <param name="entity">The value to cache.</param>
    /// <param name="expiration">The expiration time for the cached value.</param>
    public void SetValue<T>(string key, T entity, TimeSpan expiration)
    {
        using var client = redisClientManager.GetClient();

        client.Set(key, entity, expiration);
    }

    /// <summary>
    /// Removes a cached value associated with the specified key.
    /// </summary>
    /// <param name="key">The key associated with the cached value to remove.</param>
    public void Remove(string key)
    {
        using var client = redisClientManager.GetClient();

        client.Remove(key);
    }

    /// <summary>
    /// Disposes of resources used by the RedisCacheService.
    /// </summary>
    protected override void DisposeServices()
    {
        redisClientManager?.Dispose();
    }
}