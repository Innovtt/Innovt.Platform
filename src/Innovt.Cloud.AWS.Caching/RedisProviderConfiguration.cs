// Innovt Company
// Author: Michel Borges
// Project: Innovt.Cloud.AWS.Caching

using System.Collections.Generic;

namespace Innovt.Cloud.AWS.Caching;

public class RedisProviderConfiguration
{
    public IEnumerable<string> ReadWriteHosts { get; set; }
    public IEnumerable<string> ReadOnlyHosts { get; set; }
    public int PoolTimeOutInSeconds { get; set; }
    public int? ConnectTimeout { get; set; }
}