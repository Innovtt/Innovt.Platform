using Innovt.Core.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Core.Caching;

public interface ICacheService
{
    T GetValue<T>(string key);

    public async Task<T> GetValue<T>(string key, Func<CancellationToken, Task<T>> factory, CancellationToken cancellationToken)
    {
        if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        var value = this.GetValue<T>(key);

        if (value != null)
            return value;

        value = await factory(cancellationToken).ConfigureAwait(false);

        return value;
    }

    public async Task<T> GetValueOrCreate<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan expiration, CancellationToken cancellationToken)
    {
        if (key.IsNullOrEmpty()) throw new ArgumentNullException(nameof(key));
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        var value = GetValue<T>(key);

        if (value is { })
            return value;

        value = await factory(cancellationToken).ConfigureAwait(false);

        if (value is { })
        {
            SetValue(key, value, expiration);
        }

        return value;
    }

    void SetValue<T>(string key, T entity, TimeSpan expiration);
    void Remove(string key);
}