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
internal sealed class BatchChangeTrackingIntegrationTests
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
    public async Task AddRangeAsync_AllUnchanged_SkipsDynamoWrite()
    {
        var entities = new List<TrackingTestEntity>
        {
            NewEntity("batch-skip-1", "a"),
            NewEntity("batch-skip-2", "b"),
            NewEntity("batch-skip-3", "c")
        };
        await repository.AddRangeAsync(entities);

        var loaded = new List<TrackingTestEntity>();
        foreach (var e in entities)
            loaded.Add((await repository.GetByIdAsync<TrackingTestEntity>(e.Pk, e.Sk))!);

        recordedActivities.Clear();
        await repository.AddRangeAsync(loaded);

        Assert.That(SkippedActivityPresent(), Is.True);
    }

    [Test]
    public async Task AddRangeAsync_MixedStates_WritesOnlyChanged()
    {
        var entities = new List<TrackingTestEntity>
        {
            NewEntity("batch-mixed-1", "a"),
            NewEntity("batch-mixed-2", "b"),
            NewEntity("batch-mixed-3", "c")
        };
        await repository.AddRangeAsync(entities);

        var loaded = new List<TrackingTestEntity>();
        foreach (var e in entities)
            loaded.Add((await repository.GetByIdAsync<TrackingTestEntity>(e.Pk, e.Sk))!);

        loaded[1].Name = "modified";

        recordedActivities.Clear();
        await repository.AddRangeAsync(loaded);

        var reloaded0 = await repository.GetByIdAsync<TrackingTestEntity>(entities[0].Pk, entities[0].Sk);
        var reloaded1 = await repository.GetByIdAsync<TrackingTestEntity>(entities[1].Pk, entities[1].Sk);
        var reloaded2 = await repository.GetByIdAsync<TrackingTestEntity>(entities[2].Pk, entities[2].Sk);

        Assert.Multiple(() =>
        {
            Assert.That(SkippedActivityPresent(), Is.False);
            Assert.That(MessagesToWriteTag(), Is.EqualTo(1));
            Assert.That(reloaded0!.Name, Is.EqualTo("a"));
            Assert.That(reloaded1!.Name, Is.EqualTo("modified"));
            Assert.That(reloaded2!.Name, Is.EqualTo("c"));
        });
    }

    [Test]
    public async Task AddRangeAsync_AddedModifiedUnchangedMixed_WritesOnlyAddedAndModified()
    {
        var seed = new List<TrackingTestEntity>
        {
            NewEntity("batch-trio-unchanged", "u"),
            NewEntity("batch-trio-modified", "m-original")
        };
        await repository.AddRangeAsync(seed);

        var unchanged = (await repository.GetByIdAsync<TrackingTestEntity>(seed[0].Pk, seed[0].Sk))!;
        var modified = (await repository.GetByIdAsync<TrackingTestEntity>(seed[1].Pk, seed[1].Sk))!;
        modified.Name = "m-updated";

        var added = NewEntity("batch-trio-added", "a");

        var batch = new List<TrackingTestEntity> { unchanged, modified, added };

        recordedActivities.Clear();
        await repository.AddRangeAsync(batch);

        var reloadedUnchanged =
            await repository.GetByIdAsync<TrackingTestEntity>(unchanged.Pk, unchanged.Sk);
        var reloadedModified =
            await repository.GetByIdAsync<TrackingTestEntity>(modified.Pk, modified.Sk);
        var reloadedAdded =
            await repository.GetByIdAsync<TrackingTestEntity>(added.Pk, added.Sk);

        Assert.Multiple(() =>
        {
            Assert.That(SkippedActivityPresent(), Is.False);
            Assert.That(MessagesToWriteTag(), Is.EqualTo(2));
            Assert.That(reloadedUnchanged!.Name, Is.EqualTo("u"));
            Assert.That(reloadedModified!.Name, Is.EqualTo("m-updated"));
            Assert.That(reloadedAdded, Is.Not.Null);
            Assert.That(reloadedAdded!.Name, Is.EqualTo("a"));
        });
    }

    [Test]
    public async Task AddRangeAsync_ResnapshotsAfterWrite_SecondCallSkipsAll()
    {
        var entities = new List<TrackingTestEntity>
        {
            NewEntity("batch-resnap-1", "a"),
            NewEntity("batch-resnap-2", "b")
        };

        await repository.AddRangeAsync(entities);

        recordedActivities.Clear();
        await repository.AddRangeAsync(entities);

        Assert.That(SkippedActivityPresent(), Is.True);
    }

    [Test]
    public async Task AddRangeAsync_NewEntities_WritesAll()
    {
        var entities = new List<TrackingTestEntity>
        {
            NewEntity("batch-new-1", "a"),
            NewEntity("batch-new-2", "b")
        };

        recordedActivities.Clear();
        await repository.AddRangeAsync(entities);

        var reloaded0 = await repository.GetByIdAsync<TrackingTestEntity>(entities[0].Pk, entities[0].Sk);
        var reloaded1 = await repository.GetByIdAsync<TrackingTestEntity>(entities[1].Pk, entities[1].Sk);

        Assert.Multiple(() =>
        {
            Assert.That(SkippedActivityPresent(), Is.False);
            Assert.That(MessagesToWriteTag(), Is.EqualTo(2));
            Assert.That(reloaded0, Is.Not.Null);
            Assert.That(reloaded1, Is.Not.Null);
        });
    }

    [Test]
    public async Task AddRangeAsync_TrackingDisabled_AlwaysWrites()
    {
        var entities = new List<TrackingTestEntity>
        {
            NewEntity("batch-off-1", "a"),
            NewEntity("batch-off-2", "b")
        };
        await repository.AddRangeAsync(entities);

        var loaded = new List<TrackingTestEntity>();
        foreach (var e in entities)
            loaded.Add((await repository.GetByIdAsync<TrackingTestEntity>(e.Pk, e.Sk))!);

        repository.EnableChangeTracking = false;

        recordedActivities.Clear();
        await repository.AddRangeAsync(loaded);

        Assert.Multiple(() =>
        {
            Assert.That(SkippedActivityPresent(), Is.False);
            Assert.That(MessagesToWriteTag(), Is.EqualTo(2));
        });
    }

    [Test]
    public async Task AddRangeAsync_NestedObjectMutated_PersistsToDynamo()
    {
        var entity = NewEntity("batch-nested", "n");
        entity.Home = new TrackingAddress { Street = "Old", City = "A", ZipCode = 10000 };
        await repository.AddRangeAsync(new List<TrackingTestEntity> { entity });

        var loaded = await repository.GetByIdAsync<TrackingTestEntity>(entity.Pk, entity.Sk);
        loaded!.Home!.Street = "New";

        recordedActivities.Clear();
        await repository.AddRangeAsync(new List<TrackingTestEntity> { loaded });

        var reloaded = await repository.GetByIdAsync<TrackingTestEntity>(entity.Pk, entity.Sk);

        Assert.Multiple(() =>
        {
            Assert.That(SkippedActivityPresent(), Is.False);
            Assert.That(reloaded!.Home!.Street, Is.EqualTo("New"));
        });
    }

    [Test]
    public async Task AddAsync_UnchangedEntity_SkipsDynamoWrite()
    {
        var entity = NewEntity("single-skip", "original");
        await repository.AddAsync(entity);

        var loaded = await repository.GetByIdAsync<TrackingTestEntity>(entity.Pk, entity.Sk);

        recordedActivities.Clear();
        await repository.AddAsync(loaded!);

        Assert.That(SkippedActivityPresent(), Is.True);
    }

    private bool SkippedActivityPresent() =>
        recordedActivities.Any(a => a.GetTagItem("Skipped") is true);

    private int MessagesToWriteTag() =>
        recordedActivities
            .Select(a => a.GetTagItem("MessagesToWrite"))
            .OfType<int>()
            .DefaultIfEmpty(0)
            .Max();

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
