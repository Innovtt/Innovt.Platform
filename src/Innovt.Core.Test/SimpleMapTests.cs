using Innovt.Core.Serialization;
using Innovt.Core.Utilities;
using NUnit.Framework;

namespace Innovt.Core.Test;

internal class SomeDomain
{
    public string Name { get; set; }

    public string Description { get; set; }
}

internal class SomeDomain2:SomeDomain
{
    public int Age { get; set; }
}

internal class SomeDto
{
    public string Name { get; set; }

    public string Description { get; set; }
}

internal class SomeDto2:SomeDto
{
    public int Age { get; set; }
}

[TestFixture]
public class SimpleMapTests
{
    
    [Test]
    public void MapReturnsNullWhenInputIsNull()
    {
        var output = SimpleMapper.Map<SomeDto, SomeDomain>(null!);

        Assert.IsNull(output);
    }


    [Test]
    public void MapReturnsAvailableProperties()
    {
        var b = new SomeDto() { Name = "michel", Description = "something" };
        
        var output = SimpleMapper.Map<SomeDto,SomeDomain>(b);

        Assert.IsNotNull(output);
        Assert.AreEqual(b.Name,output.Name);
        Assert.AreEqual(b.Description,output.Description);
    }

    [Test]
    public void MapReturnsPropertiesThatExistsOnlyInInput()
    {
        var b = new SomeDto2() { Name = "michel", Description = "something",Age = 2};
        
        var output = SimpleMapper.Map<SomeDto2,SomeDomain>(b);

        Assert.IsNotNull(output);
        Assert.AreEqual(b.Name,output.Name);
        Assert.AreEqual(b.Description,output.Description);
    }

    [Test]
    public void MapKeepPropertyDefaultIfThePropertyWasNotFoundInTheInput()
    {
        var b = new SomeDto() { Name = "michel", Description = "something"};
        
        var output = SimpleMapper.Map<SomeDto,SomeDomain2>(b);

        Assert.IsNotNull(output);
        Assert.AreEqual(b.Name,output.Name);
        Assert.AreEqual(b.Description,output.Description);
        Assert.AreEqual(0,output.Age);
    }


    [Test]
    public void MapWithInstances()
    {
        var a = new SomeDto() { Name = "michel", Description = "something"};
        var b = new SomeDomain2() { };
        
        SimpleMapper.Map<SomeDto,SomeDomain2>(a,b);

        Assert.IsNotNull(b);
        Assert.AreEqual(a.Name,b.Name);
        Assert.AreEqual(a.Description,b.Description);
    }
}