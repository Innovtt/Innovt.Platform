// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core.Test

using System;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Caching;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;

namespace Innovt.Core.Test;

[TestFixture]
public class LocalCacheTests
{
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

    private ICacheService cacheService;

    [Test]
    public void GetValueThrowExceptionIfKeyIsNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => cacheService.GetValue<int>(null));

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await cacheService.GetValue(null, token => Task.FromResult(10), CancellationToken.None)
                .ConfigureAwait(false));

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await cacheService
                .GetValueOrCreate(null, token => Task.FromResult(10), TimeSpan.FromSeconds(10), CancellationToken.None)
                .ConfigureAwait(false));
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

        Assert.That(0, Is.EqualTo(value));

        var value2 = cacheService.GetValue<object>("User");

        Assert.That(value2, Is.Null);
    }


    [Test]
    public async Task GetValueWithFactoryReturnsFactoryValue()
    {
        var value = await cacheService.GetValue<int?>("Quantity", Factory, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.That(10, Is.EqualTo(value));
    }


    [Test]
    public async Task GetValueOrCreate()
    {
        var expiration = TimeSpan.FromSeconds(60);
        var key = "Quantity";
        var expectedValue = 10;

        var value = await cacheService
            .GetValueOrCreate<int?>(key, (c) => { return FactoryB(null, c); }, expiration, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.That(expectedValue, Is.EqualTo(value));

        await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);

        value = cacheService.GetValue<int>(key);

        Assert.That(expectedValue, Is.EqualTo(value));
    }


    //Only for example 
    private static Task<int?> Factory(CancellationToken cancellation)
    {
        return Task.FromResult<int?>(10);
    }

    private static Task<int?> FactoryB(object a, CancellationToken cancellation = default)
    {
        return Task.FromResult<int?>(10);
    }

    [Test]
    public void SetValue()
    {
        var key = "Quantity";
        var expectedValue = 100;

        cacheService.SetValue<int>(key, expectedValue, TimeSpan.FromDays(1));

        var value = cacheService.GetValue<int>(key);

        Assert.That(expectedValue, Is.EqualTo(value));
    }

    [Test]
    public async Task SetValueWithSecondsShouldReturnNullTask()
    {
        var key = "Quantity";
        var expectedValue = 0;
        var expiration = TimeSpan.FromSeconds(1);

        cacheService.SetValue<int>(key, 100, TimeSpan.FromSeconds(1));

        await Task.Delay(expiration * 2).ConfigureAwait(false);

        var value = cacheService.GetValue<int>(key);

        Assert.That(expectedValue, Is.EqualTo(value));
    }


    [Test]
    public void Remove()
    {
        var key = "Quantity";
        var expectedValue = 0;

        cacheService.SetValue<int>(key, 100, TimeSpan.FromDays(1));

        cacheService.Remove(key);

        var value = cacheService.GetValue<int>(key);
        Assert.That(expectedValue, Is.EqualTo(value));
    }
}