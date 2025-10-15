using Innovt.Core.CrossCutting.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace Innovt.CrossCutting.IOC.Tests.Mock;

public class IocModule1 : IocModule
{
    public IocModule1() : base()
    {
        GetServices().AddTransient<IServiceA, ServiceA>();
    }

    public IocModule1(IServiceCollection services) : base(services)
    {
        services.AddTransient<IServiceA, ServiceA>();

        // Custom initialization logic for IocModule1 can be added here
    }
}