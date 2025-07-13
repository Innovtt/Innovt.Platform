using Innovt.CrossCutting.IOC.Tests.Mock;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.CrossCutting.IOC.Tests;

[TestFixture]
public class Tests
{
    [Test]
    public void AddModules_Should_RegisterServices_WithoutServiceCollection()
    {
        var container = new Container();
        
        container.AddModule(new IocModule1()).AddModule(new IocModule2());
        
        var serviceA = container.Resolve<IServiceA>();
        var serviceB = container.Resolve<IServiceB>();
        Assert.Multiple(() =>
        {
            Assert.That(serviceA, Is.Not.Null);
            Assert.That(serviceB, Is.Not.Null);
        });
        
        container.CheckConfiguration();
    }
    
    [Test]
    public void AddModules_Should_RegisterServices_WhenAServiceCollectionIsPassed()
    {
        var services = new ServiceCollection();
        
        var container = new Container(services);

        container.AddModule(new IocModule1(services)).AddModule(new IocModule2());
        
        var serviceA = container.Resolve<IServiceA>();
        var serviceB = container.Resolve<IServiceB>();
        
        Assert.Multiple(() =>
        {
            Assert.That(serviceB, Is.Not.Null);
            Assert.That(serviceA, Is.Not.Null);
        });
    }
    
    [Test]
    public void AddModules_Should_RegisterServices_WithContainerServiceCollection()
    {
        var container = new Container(new ServiceCollection());

        container.AddModule(new IocModule1()).AddModule(new IocModule2());
        
        var serviceA = container.Resolve<IServiceA>();
        var serviceB = container.Resolve<IServiceB>();
        
        Assert.Multiple(() =>
        {
            Assert.That(serviceB, Is.Not.Null);
            Assert.That(serviceA, Is.Not.Null);
        });
    }
}