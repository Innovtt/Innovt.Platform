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
using NSubstitute;
using NUnit.Framework;

namespace Innovt.Core.Test;

[TestFixture]
public class MultiLayerCacheServiceTests
{
    [SetUp]
    public void Setup()
    {
        loggerMock = Substitute.For<ILogger>();

        var defaultCache = new LocalCache(new MemoryCache(new MemoryCacheOptions { CompactionPercentage = 1 }));

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
        Assert.Throws<ArgumentNullException>(() => cacheService.SetValue(null, 0, TimeSpan.FromSeconds(10)));
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
        var value = await cacheService.GetValue("Quantity", Factory, CancellationToken.None).ConfigureAwait(false);

        Assert.That(value, Is.Not.Null);
        Assert.That("Michel", Is.EqualTo(value.Name));
    }


    [Test]
    public async Task GetValueOrCreate()
    {
        var expiration = TimeSpan.FromSeconds(60);
        var key = "Quantity";
        var expectedValue = "Michel";

        var value = await cacheService.GetValueOrCreate(key, Factory, expiration, CancellationToken.None)
            .ConfigureAwait(false);

        Assert.That(value, Is.Not.Null);
        Assert.That(expectedValue, Is.EqualTo(value.Name));

        Thread.Sleep(TimeSpan.FromSeconds(2));

        value = cacheService.GetValue<A>(key);

        Assert.That(value, Is.Not.Null);
        Assert.That(expectedValue, Is.EqualTo(value.Name));
    }

    public static Task<A> Factory(CancellationToken arg)
    {
        return Task.FromResult(new A
        {
            Name = "Michel"
        });
    }

    [Test]
    public void SetValue()
    {
        var key = "Quantity";
        var expectedValue = 100;

        cacheService.SetValue(key, expectedValue, TimeSpan.FromDays(1));

        var value = cacheService.GetValue<int>(key);

        Assert.That(expectedValue, Is.EqualTo(value));
    }

    [Test]
    public void SetValueWithSecondsShouldReturnNullTask()
    {
        var key = "Quantity";
        var expectedValue = 0;
        var expiration = TimeSpan.FromSeconds(1);

        cacheService.SetValue(key, 100, TimeSpan.FromSeconds(1));

        Thread.Sleep(expiration * 2);

        var value = cacheService.GetValue<int>(key);

        Assert.That(expectedValue, Is.EqualTo(value));
    }


    [Test]
    public void Remove()
    {
        var key = "Quantity";
        var expectedValue = 0;

        cacheService.SetValue(key, 100, TimeSpan.FromDays(1));

        cacheService.Remove(key);

        var value = cacheService.GetValue<int>(key);

        Assert.That(expectedValue, Is.EqualTo(value));
    }
}