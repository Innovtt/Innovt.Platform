// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core.Test

using System;
using System.Threading;
using System.Threading.Tasks;
using Innovt.Core.Caching;
using Innovt.Core.CrossCutting.Log;
using Innovt.Core.Test.Models;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;

namespace Innovt.Core.Test;

[TestFixture]
public class MultiLayerCacheServiceTests
{
    [SetUp]
    public void Setup()
    {
        loggerMock = NSubstitute.Substitute.For<ILogger>();

        var defaultCache = new LocalCache(new MemoryCache(new MemoryCacheOptions() { CompactionPercentage = 1 }));

        cacheService = new MultiLayerCacheService(defaultCache, loggerMock);
    }


    [TearDown]
    public void TearDown()
    {
        cacheService = null;
    }

    private ILogger loggerMock;
    private ICacheService cacheService;

    [Test]
    public void GetValueThrowExceptionIfKeyIsNullOrEmpty()
    {
        Assert.Throws<ArgumentNullException>(() => cacheService.GetValue<int>(null));

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await cacheService.GetValue(null, Factory, CancellationToken.None).ConfigureAwait(false));

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await cacheService.GetValueOrCreate(null, Factory, TimeSpan.FromSeconds(10), CancellationToken.None)
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

        Assert.AreEqual(0, value);

        var value2 = cacheService.GetValue<object>("User");

        Assert.IsNull(value2);
    }


    [Test]
    public async Task GetValueWithFactoryReturnsFactoryValue()
    {
        var value = await cacheService.GetValue<A>("Quantity", Factory, CancellationToken.None).ConfigureAwait(false);

        Assert.IsNotNull(value);
        Assert.AreEqual("Michel", value.Name);
    }


    [Test]
    public async Task GetValueOrCreate()
    {
        var expiration = TimeSpan.FromSeconds(60);
        var key = "Quantity";
        var expectedValue = "Michel";

        var value = await cacheService.GetValueOrCreate<A>(key, Factory, expiration, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.IsNotNull(value);
        Assert.AreEqual(expectedValue, value.Name);

        Thread.Sleep(TimeSpan.FromSeconds(2));

        value = cacheService.GetValue<A>(key);

        Assert.IsNotNull(value);
        Assert.AreEqual(expectedValue, value.Name);
    }

    public Task<A> Factory(CancellationToken arg)
    {
        return Task.FromResult(new A()
        {
            Name = "Michel"
        });
    }

    [Test]
    public async Task SetValue()
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