// Innovt Company
// Author: Michel Borges
// Project: Innovt.Core.Test

using System;
using Innovt.Core.Serialization;
using Innovt.Core.Test.Models;
using NUnit.Framework;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Innovt.Core.Test;

public class DeserializerFactoryTests
{
    [Test]
    public void InstanceShouldNotReturnNUll()
    {
        var instance = DeserializerFactory.Instance;

        Assert.That(instance, Is.Not.Null);
    }

    [Test]
    public void InstanceShouldBeTheSameAfterManyCalls()
    {
        var instanceA = DeserializerFactory.Instance;

        var instanceB = DeserializerFactory.Instance;

        Assert.That(instanceA, Is.Not.Null);
        Assert.That(instanceB, Is.Not.Null);
        Assert.That(instanceA, Is.EqualTo(instanceB));
    }

    [Test]
    public void DeserializeShouldReturnNullIfContentIsNullOrEmpty()
    {
        var result = DeserializerFactory.Instance.Deserialize("A", null);


        Assert.That(result, Is.Null);

        result = DeserializerFactory.Instance.Deserialize("A", "");

        Assert.That(result, Is.Null);
    }


    [Test]
    public void DeserializeShouldReturnNullWhenHasNoMapping()
    {
        var result = DeserializerFactory.Instance.Deserialize("A", "");

        Assert.That(result, Is.Null);

        //new mapping
        DeserializerFactory.Instance.AddMapping<A>("A");

        var result2 = DeserializerFactory.Instance.Deserialize("B", "B");

        Assert.That(result2, Is.Null);
    }


    [Test]
    public void Deserialize()
    {
        //first check without mapping
        DeserializerFactory.Instance.AddMapping<A>("A").AddMapping<B>();

        var a = new A { Age = 10, LastName = "Borges", Name = "Michel" };
        var aJsonContent = JsonSerializer.Serialize(a);

        //With Key 
        var resultA = DeserializerFactory.Instance.Deserialize("A", aJsonContent);

        Assert.That(resultA, Is.Not.Null);
        Assert.That(resultA as A, Is.Not.Null);

        var b = new B { Document = "12312", Id = Guid.Empty, Role = "Admin" };
        var bJsonContent = JsonSerializer.Serialize(b);

        //With Key 
        var resultB = DeserializerFactory.Instance.Deserialize(typeof(B).FullName, bJsonContent);

        Assert.That(resultB, Is.Not.Null);
        Assert.That(resultB as B, Is.Not.Null);
    }
}