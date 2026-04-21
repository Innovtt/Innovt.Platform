using System;
using System.Collections.Generic;
using Innovt.Cloud.AWS.Dynamo.ChangeTracking;
using NUnit.Framework;

namespace Innovt.Cloud.AWS.Dynamo.Tests.ChangeTracking;

[TestFixture]
public class ChangeTrackerTests
{
    private ChangeTracker tracker = null!;

    [SetUp]
    public void SetUp() => tracker = new ChangeTracker();

    [Test]
    public void GetState_ForEntityNeverAttached_ReturnsAdded()
    {
        var entity = new ScalarEntity { Id = "1", Name = "Alice" };

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Added));
    }

    [Test]
    public void GetState_ForDifferentInstanceWithSameData_ReturnsAdded()
    {
        var attached = new ScalarEntity { Id = "1", Name = "Alice" };
        tracker.Attach(attached);

        var differentInstance = new ScalarEntity { Id = "1", Name = "Alice" };

        Assert.That(tracker.GetState(differentInstance), Is.EqualTo(EntityState.Added));
    }

    [Test]
    public void GetState_AfterAttachWithNoMutation_ReturnsUnchanged()
    {
        var entity = new ScalarEntity { Id = "1", Name = "Alice", Counter = 5 };
        tracker.Attach(entity);

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Unchanged));
    }

    [Test]
    public void GetState_AfterMutatingJsonIgnoredProperty_ReturnsUnchanged()
    {
        var entity = new ScalarEntity { Id = "1", Name = "Alice" };
        tracker.Attach(entity);

        entity.Ephemeral = "transient";

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Unchanged));
    }

    [Test]
    public void Attach_Twice_RebaselinesSnapshot()
    {
        var entity = new ScalarEntity { Id = "1", Name = "Alice", Counter = 1 };
        tracker.Attach(entity);

        entity.Counter = 99;
        tracker.Attach(entity);

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Unchanged));
    }

    [Test]
    public void GetState_AfterMutatingString_ReturnsModified()
    {
        var entity = new ScalarEntity { Name = "Alice" };
        tracker.Attach(entity);

        entity.Name = "Bob";

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Modified));
    }

    [Test]
    public void GetState_AfterMutatingInt_ReturnsModified()
    {
        var entity = new ScalarEntity { Counter = 5 };
        tracker.Attach(entity);

        entity.Counter = 6;

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Modified));
    }

    [Test]
    public void GetState_AfterMutatingDecimal_ReturnsModified()
    {
        var entity = new ScalarEntity { Price = 9.99m };
        tracker.Attach(entity);

        entity.Price = 10.00m;

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Modified));
    }

    [Test]
    public void GetState_AfterMutatingDateTime_ReturnsModified()
    {
        var entity = new ScalarEntity { CreatedAt = new DateTime(2026, 1, 1) };
        tracker.Attach(entity);

        entity.CreatedAt = new DateTime(2026, 1, 2);

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Modified));
    }

    [Test]
    public void GetState_AfterMutatingGuid_ReturnsModified()
    {
        var entity = new ScalarEntity { Token = Guid.NewGuid() };
        tracker.Attach(entity);

        entity.Token = Guid.NewGuid();

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Modified));
    }

    [Test]
    public void GetState_AfterMutatingBool_ReturnsModified()
    {
        var entity = new ScalarEntity { IsEnabled = false };
        tracker.Attach(entity);

        entity.IsEnabled = true;

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Modified));
    }

    [Test]
    public void GetState_AfterMutatingEnum_ReturnsModified()
    {
        var entity = new ScalarEntity { Level = Priority.Low };
        tracker.Attach(entity);

        entity.Level = Priority.High;

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Modified));
    }

    [Test]
    public void GetState_AfterReassigningNestedObject_ReturnsModified()
    {
        var entity = new ComplexEntity
        {
            Id = "1",
            Home = new Address { Street = "Old St", City = "A", ZipCode = 10000 }
        };
        tracker.Attach(entity);

        entity.Home = new Address { Street = "New St", City = "B", ZipCode = 20000 };

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Modified));
    }

    [Test]
    public void GetState_AfterInPlaceMutationOfNestedObject_ReturnsModified()
    {
        var entity = new ComplexEntity
        {
            Id = "1",
            Home = new Address { Street = "Old St", City = "A", ZipCode = 10000 }
        };
        tracker.Attach(entity);

        entity.Home.Street = "New St";

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Modified));
    }

    [Test]
    public void GetState_AfterAddingToCollection_ReturnsModified()
    {
        var entity = new ComplexEntity { Id = "1", Tags = new List<string> { "alpha" } };
        tracker.Attach(entity);

        entity.Tags.Add("beta");

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Modified));
    }

    [Test]
    public void GetState_AfterRemovingFromCollection_ReturnsModified()
    {
        var entity = new ComplexEntity { Id = "1", Tags = new List<string> { "alpha", "beta" } };
        tracker.Attach(entity);

        entity.Tags.RemoveAt(0);

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Modified));
    }

    [Test]
    public void GetState_AfterInPlaceMutationOfCollectionItem_ReturnsModified()
    {
        var entity = new ComplexEntity
        {
            Id = "1",
            Lines = new List<LineItem> { new() { Sku = "X", Quantity = 1 } }
        };
        tracker.Attach(entity);

        entity.Lines[0].Quantity = 2;

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Modified));
    }

    [Test]
    public void GetState_AfterReorderingCollection_ReturnsModified()
    {
        var entity = new ComplexEntity { Id = "1", Tags = new List<string> { "a", "b" } };
        tracker.Attach(entity);

        entity.Tags.Reverse();

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Modified));
    }

    [Test]
    public void GetState_EmptyCollectionsWithDifferentInstances_ReturnsUnchanged()
    {
        var entity = new ComplexEntity { Id = "1", Tags = new List<string>() };
        tracker.Attach(entity);

        entity.Tags = new List<string>();

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Unchanged));
    }

    [Test]
    public void GetState_AfterNullingReferenceProperty_ReturnsModified()
    {
        var entity = new ComplexEntity
        {
            Id = "1",
            Home = new Address { Street = "St" }
        };
        tracker.Attach(entity);

        entity.Home = null!;

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Modified));
    }

    [Test]
    public void Attach_WithDirectCycle_DoesNotStackOverflow()
    {
        var parent = new TreeNode { Name = "Parent" };
        var child = new TreeNode { Name = "Child", Parent = parent };
        parent.Children = new List<TreeNode> { child };

        Assert.DoesNotThrow(() => tracker.Attach(parent));
        Assert.That(tracker.GetState(parent), Is.EqualTo(EntityState.Unchanged));
    }

    [Test]
    public void GetState_WithCycleAndMutation_ReturnsModified()
    {
        var parent = new TreeNode { Name = "Parent" };
        var child = new TreeNode { Name = "Child", Parent = parent };
        parent.Children = new List<TreeNode> { child };

        tracker.Attach(parent);

        child.Name = "Renamed";

        Assert.That(tracker.GetState(parent), Is.EqualTo(EntityState.Modified));
    }

    [Test]
    public void GetState_WithSharedNonCyclicReference_ComparesEqually()
    {
        var shared = new Address { Street = "Shared", City = "S", ZipCode = 1 };
        var entity = new ComplexEntity { Id = "1", Home = shared };

        tracker.Attach(entity);

        Assert.That(tracker.GetState(entity), Is.EqualTo(EntityState.Unchanged));
    }

    [Test]
    public void Attach_WithNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => tracker.Attach(null!));
    }

    [Test]
    public void GetState_WithNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => tracker.GetState(null!));
    }
}
