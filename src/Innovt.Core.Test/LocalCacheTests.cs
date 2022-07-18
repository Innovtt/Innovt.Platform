using Innovt.Core.Caching;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Innovt.Core.Test;

[TestFixture]
public class LocalCacheTests
{
    private ICacheService cacheService;

    [SetUp]
    public void Setup()
    {
        cacheService = new LocalCache(new MemoryCache(new MemoryCacheOptions() { CompactionPercentage = 1 }));
    }


    [TearDown]
    public void TearDown()
    {
        cacheService = null;
    }

    [Test]
    public void GetValueThrowExceptionIfKeyIsNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => cacheService.GetValue<int>(null));

        Assert.ThrowsAsync<ArgumentNullException>(async () => await cacheService.GetValue(null, token => Task.FromResult(10), CancellationToken.None).ConfigureAwait(false));

        Assert.ThrowsAsync<ArgumentNullException>(async () => await cacheService.GetValueOrCreate(null, token => Task.FromResult(10), TimeSpan.FromSeconds(10), CancellationToken.None).ConfigureAwait(false));
    }

    [Test]
    public void SetValueThrowExceptionIfKeyIsNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => cacheService.SetValue<int>(null, 0, TimeSpan.FromSeconds(10)));
    }


    [Test]
    public void GetValueReturnDefaultIfKeyDoesNotExist()
    {
        var value = cacheService.GetValue<int>("Quantity");

        Assert.AreEqual(0, value);

        var value2 = cacheService.GetValue<object>("User");

        Assert.IsNull(value2);
    }


    [Test]
    public async Task GetValueWithFactoryReturnsFactoryValue()
    {
        var value = await cacheService.GetValue<int?>("Quantity", Factory, CancellationToken.None).ConfigureAwait(false);

        Assert.AreEqual(10, value);
    }


    [Test]
    public async Task GetValueOrCreate()
    {
        var expiration = TimeSpan.FromSeconds(60);
        var key = "Quantity";
        var expectedValue = 10;

        var value = await cacheService.GetValueOrCreate<int?>(key, Factory, expiration, CancellationToken.None).ConfigureAwait(false);

        Assert.AreEqual(expectedValue, value);

        Thread.Sleep(TimeSpan.FromSeconds(2));

        value = cacheService.GetValue<int>(key);

        Assert.AreEqual(expectedValue, value);
    }

    private async Task<int?> Factory(CancellationToken arg)
    {
        return 10;
    }

    [Test]
    public void SetValue()
    {
        var key = "Quantity";
        var expectedValue = 100;

        cacheService.SetValue<int>(key, expectedValue, TimeSpan.FromDays(1));

        var value = cacheService.GetValue<int>(key);

        Assert.AreEqual(expectedValue, value);
    }

    [Test]
    public void SetValueWithSecondsShouldReturnNullTask()
    {
        var key = "Quantity";
        var expectedValue = 0;
        var expiration = TimeSpan.FromSeconds(1);

        cacheService.SetValue<int>(key, 100, TimeSpan.FromSeconds(1));

        Thread.Sleep(expiration * 2);

        var value = cacheService.GetValue<int>(key);

        Assert.AreEqual(expectedValue, value);
    }


    [Test]
    public void Remove()
    {
        var key = "Quantity";
        var expectedValue = 0;

        cacheService.SetValue<int>(key, 100, TimeSpan.FromDays(1));

        cacheService.Remove(key);

        var value = cacheService.GetValue<int>(key);

        Assert.AreEqual(expectedValue, value);
    }
}