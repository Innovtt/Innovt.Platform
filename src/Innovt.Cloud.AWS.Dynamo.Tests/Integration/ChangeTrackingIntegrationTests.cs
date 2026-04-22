using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Innovt.Cloud.AWS.Configuration;
using Innovt.Core.CrossCutting.Log;
using NSubstitute;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Dynamo.Tests.Integration;

[TestFixture]
[Category("Integration")]
internal sealed class ChangeTrackingIntegrationTests
{
    private TrackingTestRepository repository = null!;
    private ConcurrentBag<Activity> recordedActivities = null!;
    private ActivityListener activityListener = null!;

    [SetUp]
    public void SetUp()
    {
        if (!DynamoLocalFixture.Available)
            Assert.Ignore("Docker is not available on this host; integration tests skipped.");

        var logger = Substitute.For<ILogger>();
        var awsConfig = Substitute.For<IAwsConfiguration>();
        awsConfig.Region.Returns("us-east-1");
        awsConfig.GetCredential().Returns(ci => null!);

        repository = new TrackingTestRepository(logger, awsConfig, DynamoLocalFixture.ServiceUrl)
        {
            EnableChangeTracking = true
        };

        recordedActivities = new ConcurrentBag<Activity>();
        activityListener = new ActivityListener
        {
            ShouldListenTo = source => source.Name == "Innovt.Cloud.AWS.Dynamo.Repository",
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
            ActivityStopped = recordedActivities.Add
        };
        ActivitySource.AddActivityListener(activityListener);
    }

    [TearDown]
    public void TearDown()
    {
        activityListener?.Dispose();
        repository?.Dispose();
    }

    [Test]
    public async Task UpdateAsync_UnchangedEntity_SkipsDynamoWrite()
    {
        var entity = NewEntity("skip-unchanged", name: "original");
        await repository.AddAsync(entity);

        var loaded = await repository.GetByIdAsync<TrackingTestEntity>(entity.Pk, entity.Sk);

        recordedActivities.Clear();
        await repository.UpdateAsync(loaded!);

        Assert.That(SkippedActivityPresent(), Is.True);
    }

    [Test]
    public async Task UpdateAsync_ModifiedScalar_PersistsToDynamo()
    {
        var entity = NewEntity("scalar-mod", name: "original");
        await repository.AddAsync(entity);

        var loaded = await repository.GetByIdAsync<TrackingTestEntity>(entity.Pk, entity.Sk);
        loaded!.Name = "updated";
        loaded.Counter = 42;

        recordedActivities.Clear();
        await repository.UpdateAsync(loaded);

        var reloaded = await repository.GetByIdAsync<TrackingTestEntity>(entity.Pk, entity.Sk);
        Assert.Multiple(() =>
        {
            Assert.That(SkippedActivityPresent(), Is.False);
            Assert.That(reloaded!.Name, Is.EqualTo("updated"));
            Assert.That(reloaded.Counter, Is.EqualTo(42));
        });
    }

    [Test]
    public async Task UpdateAsync_NestedObjectMutatedInPlace_PersistsToDynamo()
    {
        var entity = NewEntity("nested-mod", name: "n");
        entity.Home = new TrackingAddress { Street = "Old", City = "A", ZipCode = 10000 };
        await repository.AddAsync(entity);

        var loaded = await repository.GetByIdAsync<TrackingTestEntity>(entity.Pk, entity.Sk);
        loaded!.Home!.Street = "New";

        recordedActivities.Clear();
        await repository.UpdateAsync(loaded);

        var reloaded = await repository.GetByIdAsync<TrackingTestEntity>(entity.Pk, entity.Sk);
        Assert.Multiple(() =>
        {
            Assert.That(SkippedActivityPresent(), Is.False);
            Assert.That(reloaded!.Home!.Street, Is.EqualTo("New"));
        });
    }

    [Test]
    public async Task UpdateAsync_CollectionItemAdded_PersistsToDynamo()
    {
        var entity = NewEntity("collection-mod", name: "c");
        entity.Tags = new List<string> { "alpha" };
        await repository.AddAsync(entity);

        var loaded = await repository.GetByIdAsync<TrackingTestEntity>(entity.Pk, entity.Sk);
        loaded!.Tags!.Add("beta");

        recordedActivities.Clear();
        await repository.UpdateAsync(loaded);

        var reloaded = await repository.GetByIdAsync<TrackingTestEntity>(entity.Pk, entity.Sk);
        Assert.Multiple(() =>
        {
            Assert.That(SkippedActivityPresent(), Is.False);
            Assert.That(reloaded!.Tags, Is.EquivalentTo(new[] { "alpha", "beta" }));
        });
    }

    [Test]
    public async Task UpdateAsync_TrackingDisabled_AlwaysWrites()
    {
        repository.EnableChangeTracking = false;

        var entity = NewEntity("tracking-off", name: "x");
        await repository.AddAsync(entity);
        var loaded = await repository.GetByIdAsync<TrackingTestEntity>(entity.Pk, entity.Sk);

        recordedActivities.Clear();
        await repository.UpdateAsync(loaded!);

        Assert.That(SkippedActivityPresent(), Is.False);
    }

    private bool SkippedActivityPresent() =>
        recordedActivities.Any(a => a.GetTagItem("Skipped") is true);

    private static TrackingTestEntity NewEntity(string id, string name)
    {
        return new TrackingTestEntity
        {
            Pk = $"TEST#{id}",
            Sk = "META",
            Name = name
        };
    }
}
