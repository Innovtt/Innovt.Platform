// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Caching

using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Caching;

/// <summary>
/// Represents the configuration settings for the Redis cache provider.
/// </summary>
public class RedisProviderConfiguration
{
    /// <summary>
    /// Gets or sets the list of Redis hosts that support both read and write operations.
    /// </summary>
    public IEnumerable<string> ReadWriteHosts { get; set; }

    /// <summary>
    /// Gets or sets the list of Redis hosts that support only read operations.
    /// </summary>
    public IEnumerable<string> ReadOnlyHosts { get; set; }

    /// <summary>
    /// Gets or sets the maximum time (in seconds) that a client is allowed to wait to acquire a connection from the pool.
    /// </summary>
    public int PoolTimeOutInSeconds { get; set; }

    /// <summary>
    /// Gets or sets the maximum time (in milliseconds) to wait for a connection to the Redis server.
    /// </summary>
    public int? ConnectTimeout { get; set; }
}