﻿// Innovt Company
// Author: Michel Borges
// Project: Innovt.Domain.Tests

using Innovt.Domain.Tests.Mocks;
using NUnit.Framework;

namespace Innovt.Domain.Tests;

[TestFixture]
public class EntityTests
{
    [Test]
    public void CheckDomainEvents()
    {
        var user = new UserEntity();

        var events = user.GetDomainEvents();

        Assert.That(events, Is.Empty);

        user.AddDomainEvent(new UserCreated());

        events = user.GetDomainEvents();

        Assert.That(events, Is.Not.Null);
        Assert.That(events.Count, Is.EqualTo(1));
    }
}