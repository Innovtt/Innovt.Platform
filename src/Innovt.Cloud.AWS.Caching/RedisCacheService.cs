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

public class RedisCacheService : AwsBaseService, ICacheService
{
    protected static readonly ActivitySource RedisProviderActivitySource =
        new("Innovt.Cloud.AWS.Caching.RedisCacheService");

    private readonly PooledRedisClientManager redisClientManager;

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

    public T GetValue<T>(string key)
    {
        using var client = redisClientManager.GetClient();

        return client.Get<T>(key);
    }

    public void SetValue<T>(string key, T entity, TimeSpan expiration)
    {
        using var client = redisClientManager.GetClient();

        client.Set(key, entity, expiration);
    }

    public void Remove(string key)
    {
        using var client = redisClientManager.GetClient();

        client.Remove(key);
    }


    protected override void DisposeServices()
    {
        redisClientManager?.Dispose();
    }
}