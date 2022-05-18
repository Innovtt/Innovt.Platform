// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core.Test
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

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

        Assert.IsNotNull(instance);
    }

    [Test]
    public void InstanceShouldBeTheSameAfterManyCalls()
    {
        var instanceA = DeserializerFactory.Instance;

        var instanceB = DeserializerFactory.Instance;

        Assert.IsNotNull(instanceA);
        Assert.IsNotNull(instanceB);

        Assert.AreEqual(instanceA, instanceB);
    }

    [Test]
    public void DeserializeShouldReturnNullIfContentIsNullOrEmpty()
    {
        var result = DeserializerFactory.Instance.Deserialize("A", null);

        Assert.IsNull(result);

        result = DeserializerFactory.Instance.Deserialize("A", "");

        Assert.IsNull(result);
    }


    [Test]
    public void DeserializeShouldReturnNullWhenHasNoMapping()
    {
        var result = DeserializerFactory.Instance.Deserialize("A", "");

        Assert.IsNull(result);

        //new mapping
        DeserializerFactory.Instance.AddMapping<A>("A");

        var result2 = DeserializerFactory.Instance.Deserialize("B", "B");

        Assert.IsNull(result2);
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

        Assert.IsNotNull(resultA);
        Assert.IsNotNull(resultA as A);

        var b = new B { Document = "12312", Id = Guid.Empty, Role = "Admin" };
        var bJsonContent = JsonSerializer.Serialize(b);

        //With Key 
        var resultB = DeserializerFactory.Instance.Deserialize(typeof(B).FullName, bJsonContent);

        Assert.IsNotNull(resultB);
        Assert.IsNotNull(resultB as B);
    }
}